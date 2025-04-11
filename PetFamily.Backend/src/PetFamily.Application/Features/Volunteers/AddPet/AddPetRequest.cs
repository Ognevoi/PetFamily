using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Domain.PetManagement.Enums;

namespace PetFamily.Application.Features.Volunteers.AddPet;


public record AddPetRequest(
    Guid VolunteerId,
    AddPetDto Dto
);

public record AddPetDto(
    string Name,
    Guid SpecieId,
    Guid BreedId,
    string Description,
    string PetColor,
    string PetHealth,
    double Weight,
    double Height,
    bool IsSterilized,
    bool IsVaccinated,
    AddressDto Address,
    DateTime BirthDate,
    PetStatus PetStatus);
    
