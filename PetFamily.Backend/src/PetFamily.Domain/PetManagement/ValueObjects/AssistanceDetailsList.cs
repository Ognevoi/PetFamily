namespace PetFamily.Domain.PetManagement.ValueObjects;

public record AssistanceDetailsList
{
    public IReadOnlyList<AssistanceDetails> AssistanceDetails { get; }
    
    // required by EF Core
    private AssistanceDetailsList()
    {
        AssistanceDetails = new List<AssistanceDetails>();
    }
    
    public AssistanceDetailsList(IEnumerable<AssistanceDetails> assistanceDetails)
    {
        AssistanceDetails = assistanceDetails.ToList();
    }
}