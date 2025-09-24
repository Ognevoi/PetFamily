using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Position : ComparableValueObject
{
    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> Forward()
        => Create(Value + 1);

    public Result<Position, Error> Back()
        => Create(Value - 1);

    public static Result<Position, Error> Create(int number)
    {
        if (number.GetType() != typeof(int))
            return Errors.General.InvalidType(nameof(Position), number.GetType().Name);


        if (number < 1)
            return Errors.General.ValueIsInvalid("Serial number");


        return new Position(number);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator int(Position position) => position.Value;
}