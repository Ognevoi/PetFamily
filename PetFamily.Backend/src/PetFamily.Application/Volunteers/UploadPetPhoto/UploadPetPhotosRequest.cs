using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.UploadPetPhoto;

public record UploadPetPhotosRequest(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Photos);