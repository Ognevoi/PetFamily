using PetFamily.Application.Features.Volunteers.AddPet;
using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Domain.PetManagement.Enums;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record AddPetRequest(
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

public static class AddPetRequestExtensions
{
    public static AddPetCommand ToCommand(this AddPetRequest request, Guid volunteerId)
        => new AddPetCommand(
            volunteerId,
            request.Name,
            request.SpecieId,
            request.BreedId,
            request.Description,
            request.PetColor,
            request.PetHealth,
            request.Weight,
            request.Height,
            request.IsSterilized,
            request.IsVaccinated,
            request.Address,
            request.BirthDate,
            request.PetStatus);
}