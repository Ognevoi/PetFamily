using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Application.Species;
using PetFamily.Application.Volunteers.Update;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;


namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public AddPetHandler(
        IFilesProvider filesProvider,
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        ILogger<UpdateVolunteerHandler> logger
    )
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(
        AddPetRequest command,
        CancellationToken cancellationToken = default)
    {
        
        var volunteerResult = await _volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId));

        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId);

        var petId = PetId.NewPetId().Value;
        
        var specieResult = await _speciesRepository.GetById(command.Dto.SpecieId, cancellationToken);
        if (specieResult.IsFailure)
            return Errors.General.NotFound(command.Dto.SpecieId);

        Console.WriteLine(specieResult.Value.ToString());

        var breedResult = specieResult.Value.GetBreed(command.Dto.BreedId);
        if (breedResult.IsFailure)
            return Errors.General.NotFound(command.Dto.BreedId);

        var petNameResult = PetName.Create(command.Dto.Name).Value;

        var petDescriptionResult = Description.Create(command.Dto.Description).Value;

        var petColorResult = PetColor.Create(command.Dto.PetColor).Value;

        var petHealthInfoResult = PetHealthInfo.Create(command.Dto.PetHealth).Value;

        var petWeightResult = Weight.Create(command.Dto.Weight).Value;

        var petHeightResult = Height.Create(command.Dto.Height).Value;

        var petIsSterilizedResult = IsSterilized.Create(command.Dto.IsSterilized).Value;

        var petIsVaccinatedResult = IsVaccinated.Create(command.Dto.IsVaccinated).Value;
        
        var petAddressResult = Address.Create(
            command.Dto.Address.Street, command.Dto.Address.City, command.Dto.Address.State, command.Dto.Address.ZipCode).Value;
        
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
            command.Dto.BirthDate,
            command.Dto.PetStatus
        );

        volunteerResult.Value.AddPet(pet);

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Pet with id {PetId} added to volunteer with id {VolunteerId}", pet.Id.Value,
            volunteerResult.Value.Id);

        return result.ToString();
    }
};
