using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.DeletePetPhoto;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<string> PhotoNames)
    : ICommand<IEnumerable<string>>;