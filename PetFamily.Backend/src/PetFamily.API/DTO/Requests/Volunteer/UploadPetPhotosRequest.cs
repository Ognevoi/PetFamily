using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Features.Volunteers.UploadPetPhoto;
using PetFamily.Application.Interfaces;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record UploadPetPhotosRequest();

public static class UploadPetPhotosRequestExtensions
{
    public static UploadPetPhotosCommand ToCommand(
        this UploadPetPhotosRequest request,
        IEnumerable<UploadFileDto> photos,
        Guid volunteerId,
        Guid petId)
    => new UploadPetPhotosCommand(volunteerId, petId, photos);
    
}
    