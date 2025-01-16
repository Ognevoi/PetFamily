namespace PetFamily.Domain.Volunteers.ValueObjects;

public record AssistanceDetailsList
{
    public IReadOnlyList<AssistanceDetails> AssistanceDetails { get; }
    
    public AssistanceDetailsList(List<AssistanceDetails> assistanceDetails)
    {
        AssistanceDetails = assistanceDetails;
    }
}