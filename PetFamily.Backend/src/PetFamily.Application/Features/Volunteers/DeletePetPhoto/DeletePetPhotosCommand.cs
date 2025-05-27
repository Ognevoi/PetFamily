using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.DeletePetPhoto;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<string> PhotoNames)
    : ICommand;