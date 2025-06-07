using PetFamily.Application.Features.Volunteers.Queries.GetPetById;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record GetPetByIdRequest(Guid? Id)
{
    public GetPetByIdQuery ToQuery() => new(Id);
}