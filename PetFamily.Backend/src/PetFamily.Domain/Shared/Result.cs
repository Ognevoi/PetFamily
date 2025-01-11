namespace PetFamily.Domain.Shared;

public class Result
{
    public Result(bool isSuccess, string? error)
    {
        if (IsSuccess && error != null)
            throw new InvalidOperationException("Invalid operation. Error message should be null for successful result");
        
        if (IsSuccess == false && error == null)
            throw new InvalidOperationException("Invalid operation. Error message should be not null for failed result");
        
        IsSuccess = isSuccess;
        Error = error;
    }

    public string? Error { get; set; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, null);
    
    public static Result Failure(string error) => new Result(false, error);
    
    public static implicit operator Result(string error) => new(false, error);
}

public class Result<TValue> : Result
{
    public Result(TValue value, bool isSuccess, string? error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    private readonly TValue _value;
    
    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("No value for failure result");
    
    public static Result<TValue> Success(TValue value) => new(value, true, null);
    
    public new static Result<TValue> Failure(string error) => new(default!, false, error);
    
    public static implicit operator Result<TValue>(TValue value) => new(value, true, null);
    
    public static implicit operator Result<TValue>(string error) => new(default!, false, error);
    
} 
 