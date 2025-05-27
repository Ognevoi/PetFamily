using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.GetPetPhoto;

public record GetPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;