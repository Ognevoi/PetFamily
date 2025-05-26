using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.UploadPetPhoto;

public record UploadPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Photos)
    : ICommand;