using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFilesProvider
{
    Task<Result<IEnumerable<string>, ErrorList>> UploadFiles(
        IEnumerable<FileData> filesData,
        string bucketName,
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<ErrorList>> DeleteFiles(
        IEnumerable<string> objectsName,
        string bucketName,
        CancellationToken cancellationToken = default);
    
    Task<Result<IEnumerable<string>, ErrorList>> GetFileLink(
        IEnumerable<string> objectsName,
        string bucketName,
        CancellationToken cancellationToken = default);
}