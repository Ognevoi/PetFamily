namespace PetFamily.Application.Models;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int Page { get; init; }
    public int Size { get; init; }
    public int TotalCount { get; init; }
    public bool HasNextPage => (Page * Size) < TotalCount;
    public bool HasPreviousPage => Page > 1;
}