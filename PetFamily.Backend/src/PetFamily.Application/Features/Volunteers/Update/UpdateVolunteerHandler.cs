using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Update;

public class UpdateVolunteerHandler : ICommandHandler<Guid, UpdateVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;
    private readonly IValidator<UpdateVolunteerCommand> _validator;

    public UpdateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerHandler> logger,
        IValidator<UpdateVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullNameResult = FullName.Create(command.FullName.FirstName, command.FullName.LastName).Value;
        var emailResult = Email.Create(command.Email).Value;
        var descriptionResult = Description.Create(command.Description).Value;
        var experienceYearsResult = ExperienceYears.Create(command.ExperienceYears).Value;
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;

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