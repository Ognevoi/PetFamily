using PetFamily.Application.Features.Volunteers.GetPetPhoto;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record GetPetPhotosRequest;
    
public static class GetPetPhotosRequestExtensions
{
    public static GetPetPhotosCommand ToCommand(
        this GetPetPhotosRequest request, Guid volunteerId, Guid petId)
        => new(volunteerId, petId);
}