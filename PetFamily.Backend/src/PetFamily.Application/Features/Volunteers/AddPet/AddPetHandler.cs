using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Features.Species;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.AddPet;

public class AddPetHandler : ICommandHandler<string, AddPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetCommand> validator,
        ISpeciesRepository speciesRepository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> HandleAsync(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId));
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petId = PetId.NewPetId().Value;

        var specieResult = await _speciesRepository.GetById(command.SpecieId, cancellationToken);
        if (specieResult.IsFailure)
            return Errors.General.NotFound(command.SpecieId).ToErrorList();

        Console.WriteLine(specieResult.Value.ToString());

        var breedResult = specieResult.Value.GetBreed(command.BreedId);
        if (breedResult.IsFailure)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        var petNameResult = PetName.Create(command.Name).Value;

        var petDescriptionResult = Description.Create(command.Description).Value;

        var petColorResult = PetColor.Create(command.PetColor).Value;

        var petHealthInfoResult = PetHealthInfo.Create(command.PetHealth).Value;

        var petWeightResult = Weight.Create(command.Weight).Value;

        var petHeightResult = Height.Create(command.Height).Value;

        var petIsSterilizedResult = IsSterilized.Create(command.IsSterilized).Value;

        var petIsVaccinatedResult = IsVaccinated.Create(command.IsVaccinated).Value;

        var petAddressResult = Address.Create(
            command.Address.Street, command.Address.City, command.Address.State, command.Address.ZipCode).Value;

        var pet = new Pet(
            petId,
            petNameResult,
            specieResult.Value,
            breedResult.Value,
            petAddressResult,
            petDescriptionResult,
            petColorResult,
            petHealthInfoResult,
            petWeightResult,
            petHeightResult,
            petIsSterilizedResult,
            petIsVaccinatedResult,
            command.BirthDate,
            command.PetStatus
        );

        volunteerResult.Value.AddPet(pet);

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Pet with id {PetId} added to volunteer with id {VolunteerId}", pet.Id.Value,
            volunteerResult.Value.Id);

        return result.ToString();
    }
};