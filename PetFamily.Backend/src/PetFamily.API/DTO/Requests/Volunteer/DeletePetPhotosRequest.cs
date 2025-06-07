using PetFamily.Application.Features.Volunteers.Commands.DeletePetPhoto;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record DeletePetPhotosRequest;
    

public static class DeletePetPhotosCommandExtensions
{
    public static DeletePetPhotosCommand ToCommand(
        this DeletePetPhotosRequest request,
        Guid volunteerId,
        Guid petId,
        IEnumerable<string> photoNames)
    {
        return new DeletePetPhotosCommand(
            volunteerId,
            petId,
            photoNames);
    }
}