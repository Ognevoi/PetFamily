using PetFamily.Application.Features.Volunteers.DTO;

namespace PetFamily.Application.Features.Volunteers.UploadPetPhoto;

public record UploadPetPhotosRequest(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Photos);