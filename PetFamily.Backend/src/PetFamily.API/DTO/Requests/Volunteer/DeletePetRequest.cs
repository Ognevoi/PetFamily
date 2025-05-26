using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Features.Volunteers.HardDeletePet;
using PetFamily.Application.Interfaces;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record DeletePetRequest;

public static class DeletePetRequestExtensions
{
    public static DeletePetCommand ToCommand(this DeletePetRequest request, Guid volunteerId, Guid petId)
    => new DeletePetCommand(volunteerId, petId);
}