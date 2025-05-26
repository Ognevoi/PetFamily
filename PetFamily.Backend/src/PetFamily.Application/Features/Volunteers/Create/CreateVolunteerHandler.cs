using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Create;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> validator,
    ILogger<CreateVolunteerHandler> logger)
    : ICommandHandler<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    { 
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var fullNameResult = FullName.Create(command.FullName.FirstName, command.FullName.LastName).Value;
        
        var emailResult = Email.Create(command.Email).Value;

        var descriptionResult = Description.Create(command.Description).Value;
        
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var experienceYearsResult = ExperienceYears.Create(command.ExperienceYears).Value;
        
        var volunteer = await volunteersRepository.GetByEmail(emailResult, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.ValueAlreadyExists(nameof(Email)).ToErrorList();
            // return volunteer.Error.ToErrorList();
        
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var volunteerToCreate = Volunteer.Create(
            volunteerId,
            fullNameResult,
            emailResult,
            descriptionResult,
            experienceYearsResult,
            phoneNumberResult
        );
        
        var socialNetworksResult = command.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);
        volunteerToCreate.Value.CreateSocialNetworks(socialNetworksResult);
        
        var assistanceDetailsResult = command.AssistanceDetails.Select(ad => AssistanceDetails.Create(ad.Name, ad.Description).Value);
        volunteerToCreate.Value.CreateAssistanceDetails(assistanceDetailsResult);
        
        await volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);
        
        logger.LogInformation("Volunteer with id {VolunteerId} created", volunteerToCreate.Value.Id);

        return (Guid)volunteerToCreate.Value.Id;
    }
    
}