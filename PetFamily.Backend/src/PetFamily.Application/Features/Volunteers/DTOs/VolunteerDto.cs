namespace PetFamily.Application.Features.Volunteers.DTOs;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int ExperienceYears { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;

    public List<PetDto> Pets { get; init; } = new();

    public DateTime CreatedAt { get; init; }
    public bool IsDeleted { get; init; }
}
