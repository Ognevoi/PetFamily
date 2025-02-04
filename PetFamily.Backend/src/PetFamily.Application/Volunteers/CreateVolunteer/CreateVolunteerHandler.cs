using CSharpFunctionalExtensions;
using PetFamily.API.Controllers;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var fullNameResult = FullName.Create(request.FirstName, request.LastName).Value;
        
        var emailResult = Email.Create(request.Email).Value;

        var descriptionResult = Description.Create(request.Description).Value;
        
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber).Value;
        
        var experienceYearsResult = ExperienceYears.Create(request.ExperienceYears).Value;
        
        var volunteer = await _volunteersRepository.GetByEmail(emailResult);
        
        if (volunteer.IsSuccess)
            return Errors.General.ValueAlreadyExists("Email");
        
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var volunteerToCreate = Volunteer.Create(
            volunteerId,
            fullNameResult,
            emailResult,
            descriptionResult,
            experienceYearsResult,
            phoneNumberResult
        );
        
        var socialNetworksResult = request.SocialNetworks.Select(sn => SocialNetwork.Create(sn.Name, sn.Url).Value);
        volunteerToCreate.Value.CreateSocialNetworks(socialNetworksResult);
        
        var assistanceDetailsResult = request.AssistanceDetails.Select(ad => AssistanceDetails.Create(ad.Name, ad.Description).Value);
        volunteerToCreate.Value.CreateAssistanceDetails(assistanceDetailsResult);
        
        await _volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);

        return (Guid)volunteerToCreate.Value.Id;
    }
}