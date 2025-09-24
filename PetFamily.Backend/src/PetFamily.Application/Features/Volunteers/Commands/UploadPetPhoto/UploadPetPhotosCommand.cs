using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.UploadPetPhoto;

public record UploadPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Photos)
    : ICommand<IEnumerable<string>>;