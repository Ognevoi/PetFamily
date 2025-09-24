using PetFamily.Application.Features.Volunteers.Commands.HardDeletePet;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record DeletePetRequest;

public static class DeletePetRequestExtensions
{
    public static DeletePetCommand ToCommand(this DeletePetRequest request, Guid volunteerId, Guid petId)
        => new DeletePetCommand(volunteerId, petId);
}