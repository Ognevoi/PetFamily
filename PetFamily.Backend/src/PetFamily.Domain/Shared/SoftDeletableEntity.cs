namespace PetFamily.Domain.Shared;

public abstract class SoftDeletableEntity<TId> : CSharpFunctionalExtensions.Entity<TId> where TId : IComparable<TId>
{
    public DateTime DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public virtual void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
    }

    public virtual void Restore()
    {
        DeletedAt = default;
        IsDeleted = false;
    }
}