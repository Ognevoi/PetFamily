using System.Text.Json.Serialization;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class AssistanceDetailsList
{
    public List<AssistanceDetails> AssistanceDetails { get; private set; }

    private AssistanceDetailsList()
    {
        AssistanceDetails = new List<AssistanceDetails>();
    }

    [JsonConstructor]
    public AssistanceDetailsList(List<AssistanceDetails> assistanceDetails)
    {
        AssistanceDetails = assistanceDetails ?? new List<AssistanceDetails>();
    }
}