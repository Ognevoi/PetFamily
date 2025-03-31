using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update;

public class UpdateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var fullNameResult = FullName.Create(request.Dto.FullName.FirstName,request.Dto.FullName.LastName).Value;
        var emailResult = Email.Create(request.Dto.Email).Value;
        var descriptionResult = Description.Create(request.Dto.Description).Value;
        var experienceYearsResult = ExperienceYears.Create(request.Dto.ExperienceYears).Value;
        var phoneNumberResult = PhoneNumber.Create(request.Dto.PhoneNumber).Value;
        
        volunteerResult.Value.Update(
            fullNameResult,
            emailResult,
            descriptionResult,
            experienceYearsResult,
            phoneNumberResult
        );
        
        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Update volunteer: " +
            "first name: {FirstName}, " +
            "last name: {LastName}, " +
            "email: {Email}, " +
            "description: {Description}, " +
            "experience years: {ExperienceYears}, " +
            "phone number: {PhoneNumber}",
            volunteerResult.Value.FullName.FirstName,
            volunteerResult.Value.FullName.LastName,
            volunteerResult.Value.Email.Value,
            volunteerResult.Value.Description.Value,
            volunteerResult.Value.ExperienceYears.Value,
            volunteerResult.Value.PhoneNumber.Value);

        return result;
    }
}