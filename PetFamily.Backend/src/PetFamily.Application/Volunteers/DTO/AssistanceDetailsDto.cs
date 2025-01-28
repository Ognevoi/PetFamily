namespace PetFamily.Application.Volunteers.DTO;

public record AssistanceDetailsDto
{
    public string Name { get; }
    public string Description { get; }
    
    public AssistanceDetailsDto(string name, string description)
    {
        Name = name;
        Description = description;
    }
}


