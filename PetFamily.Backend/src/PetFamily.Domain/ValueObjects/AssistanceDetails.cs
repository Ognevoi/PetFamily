using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects;

public record AssistanceDetails
{
    private AssistanceDetails(string name, string? description)
    {
        Name = name;
        Description = description;
    }
    
    public string Name { get; }
    public string? Description { get; }
    
    public static Result<AssistanceDetails> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<AssistanceDetails>("Assistance name should not be empty");
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<AssistanceDetails>("Assistance description should not be empty");
        
        var assistanceDetails = new AssistanceDetails(name, description);
        
        return Result.Success(assistanceDetails);
    }
}