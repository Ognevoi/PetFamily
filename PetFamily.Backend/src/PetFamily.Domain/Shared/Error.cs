namespace PetFamily.Domain.Shared;

public record Error
{
    public const string SEPARATOR = "||";
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public static Error Unexpected(string code, string message) => new(code, message, ErrorType.Unexpected);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized?.Split(SEPARATOR);
        if (parts == null || parts.Length < 3)
            return Failure("Deserialization", "Invalid format");

        return Enum.TryParse(parts[2], out ErrorType type)
            ? new Error(parts[0], parts[1], type)
            : Failure("Deserialization", "Unrecognized error type");
    }

    public ErrorList ToErrorList() => new([this]);
}

public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict,
    Unexpected
}