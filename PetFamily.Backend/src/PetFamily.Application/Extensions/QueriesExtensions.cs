using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;

namespace PetFamily.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source, int page, int size, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return new PagedList<T>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalCount = count,
        };
    }

    public static IQueryable<T> WhereIfNotNullOrEmpty<T, TProperty>(
        this IQueryable<T> source,
        TProperty? value,
        Expression<Func<T, TProperty>> propertySelector)
    {
        if (value == null)
            return source;

        // Handle string separately to allow "empty string" check
        if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
            return source;

        var parameter = propertySelector.Parameters[0];
        var property = propertySelector.Body;
        var constant = Expression.Constant(value, typeof(TProperty));

        // property == value
        var equal = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

        return source.Where(lambda);
    }
}