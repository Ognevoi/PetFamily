using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.Enums;

namespace PetFamily.Application.Features.Volunteers.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
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
    PetStatus PetStatus) : ICommand;
    
