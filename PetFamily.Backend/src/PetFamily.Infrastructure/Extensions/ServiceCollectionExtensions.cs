using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static PropertyBuilder<IReadOnlyList<T>> JsonValueObjectCollectionConversion<T>(
        this PropertyBuilder<IReadOnlyList<T>> builder)
    {
        return builder.HasConversion<string>(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<IReadOnlyList<T>>(v, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<T>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()));
    }
}