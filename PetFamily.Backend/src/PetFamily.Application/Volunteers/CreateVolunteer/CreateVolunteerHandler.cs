using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    
    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
            return emailResult.Error;
        
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return phoneNumberResult.Error;
        
        var volunteer = await _volunteersRepository.GetByEmail(emailResult.Value);
        
        if (volunteer.IsSuccess)
            return Errors.General.ValueAlreadyExists("Email");
        
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var volunteerToCreate = Volunteer.Create(
            volunteerId,
            command.FullName,
            emailResult.Value,
            command.Description,
            command.ExperienceYears,
            phoneNumberResult.Value
        );
        
        var socialNetworksResult = command.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);
        volunteerToCreate.Value.CreateSocialNetworks(socialNetworksResult);
        
        var assistanceDetailsResult = command.AssistanceDetails.Select(ad => AssistanceDetails.Create(ad.Name, ad.Description).Value);
        volunteerToCreate.Value.CreateAssistanceDetails(assistanceDetailsResult);
        
        await _volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);

        return (Guid)volunteerToCreate.Value.Id;
    }
}