using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.Create;

public class CreateVolunteerHandler : ICommandHandler<CreateVolunteerCommand, Guid>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var fullNameResult = FullName.Create(command.FullName.FirstName, command.FullName.LastName).Value;

        var emailResult = Email.Create(command.Email);

        var descriptionResult = Description.Create(command.Description).Value;

        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;

        var experienceYearsResult = ExperienceYears.Create(command.ExperienceYears).Value;

        var volunteer = await _volunteersRepository.GetByEmail(emailResult.Value, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.ValueAlreadyExists(nameof(Email)).ToErrorList();

        var volunteerId = VolunteerId.NewVolunteerId();

        var volunteerToCreate = Volunteer.Create(
            volunteerId,
            fullNameResult,
            emailResult.Value,
            descriptionResult,
            experienceYearsResult,
            phoneNumberResult
        );

        var socialNetworksResult = command.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);
        volunteerToCreate.Value.CreateSocialNetworks(socialNetworksResult);

        var assistanceDetailsResult =
            command.AssistanceDetails.Select(ad => AssistanceDetails.Create(ad.Name, ad.Description).Value);
        volunteerToCreate.Value.CreateAssistanceDetails(assistanceDetailsResult);

        await _volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);

        _logger.LogInformation("Volunteer with id {VolunteerId} created", volunteerToCreate.Value.Id);

        return (Guid)volunteerToCreate.Value.Id;
    }
}