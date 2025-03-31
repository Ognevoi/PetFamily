namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label= name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid"); 
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id: {id}";
            return Error.NotFound("record.not.found", $"record not found{forId}"); 
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var label= name ?? "value";
            return Error.Validation("value.is.required", $"{label} is required"); 
        }
        
        public static Error ValueAlreadyExists(string? name = null)
        {
            var label= name ?? "value";
            return Error.Validation("value.already.exists", $"{label} already exists"); 
        }
        
        public static Error ValueIsTooLong(string? name = null, int? maxLength = null)
        {
            var label= name ?? "value";
            var length = maxLength == null ? "" : $" with max length {maxLength}";
            return Error.Validation("value.is.too.long", $"{label} is too long{length}"); 
        }
        
        public static Error UploadFailure(string message)
        {
            return Error.Unexpected("upload.failure", message);
        }
        
        public static Error DeleteFileFailure(string message)
        {
            return Error.Unexpected("delete.file.failure", message);
        }
        
        public static Error NoFileForProcessing()
        {
            return Error.Validation("no.file.for.processing", "No file for processing");
        }

    }
}