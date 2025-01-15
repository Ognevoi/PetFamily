using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record AssistanceDetailsList
{
    private static readonly List<AssistanceDetails> _assistanceDetails = new();

    public IReadOnlyList<AssistanceDetails> AssistanceDetails => _assistanceDetails;

    public static Result AddAssistanceDetails(AssistanceDetails assistanceDetails)
    {
        if (assistanceDetails == null)
            return "Assistance details is required";

        _assistanceDetails.Add(assistanceDetails);

        return Result.Success();
    }
}