using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.GetPetPhoto;

public record GetPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;