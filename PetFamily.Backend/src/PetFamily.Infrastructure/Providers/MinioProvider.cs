using System.Collections.Concurrent;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.InfrastructureConstants;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFilesProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> UploadFiles(
        IEnumerable<FileData> filesData,
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var ensureMakeBucketResult = await EnsureMakeBucket(bucketName, cancellationToken);
        if (ensureMakeBucketResult.IsFailure)
            return ensureMakeBucketResult.Error.ToErrorList();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = MinioConstants.MAX_CONCURRENCY
        };

        ConcurrentBag<string> uploadFiles = [];
        List<Error> uploadErrors = [];
        await Parallel.ForEachAsync(filesData, options, async (fileData, cancellationToken) =>
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.ObjectName);

            try
            {
                var uploadResult = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                uploadFiles.Add(uploadResult.ObjectName);
            }
            catch
            {
                uploadErrors.Add(Errors.General.UploadFailure(
                    $"Failed to upload file '{fileData.ObjectName}' to bucket '{bucketName}'"));
            }
        });

        if (uploadErrors.Count != 0)
            return new ErrorList(uploadErrors);

        _logger.LogInformation("Successfully uploaded {Count} files.", uploadFiles.Count);

        return uploadFiles;
    }

    public async Task<UnitResult<ErrorList>> DeleteFiles(
        IEnumerable<string> objectsName,
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        List<Error> deleteErrors = [];
        foreach (var objectName in objectsName)
        {
            var fileExistsResult = await IsFileExists(objectName, bucketName, cancellationToken);

            if (fileExistsResult.IsFailure)
            {
                deleteErrors.Add(fileExistsResult.Error);
                continue;
            }

            if (!fileExistsResult.Value)
            {
                continue;
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            try
            {
                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            }
            catch
            {
                deleteErrors.Add(Errors.General.DeleteFileFailure(
                    $"Failed to delete file '{objectName}' from bucket '{bucketName}'."));
            }
        }

        _logger.LogInformation("Successfully deleted {Count} files.", deleteErrors.Count);

        return UnitResult.Success<ErrorList>();
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> GetFileLink(
        IEnumerable<string> objectNames,
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        List<string> links = new();
        List<Error> getLinkErrors = new();

        foreach (var objectName in objectNames)
        {
            var fileExistsResult = await IsFileExists(objectName, bucketName, cancellationToken);
            if (fileExistsResult.IsFailure)
            {
                getLinkErrors.Add(fileExistsResult.Error);
                continue;
            }

            var preSignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(MinioConstants.DEFAULT_EXPIRY);

            try
            {
                var preSignedUrl = await _minioClient.PresignedGetObjectAsync(preSignedGetObjectArgs);

                _logger.LogInformation("Successfully generated link for file '{ObjectName}' from bucket '{BucketName}'",
                    objectName, bucketName);

                links.Add(preSignedUrl);
            }
            catch
            {
                getLinkErrors.Add(Errors.General.UploadFailure(
                    $"Failed to generate link for file '{objectName}' from bucket '{bucketName}'"));
            }
        }

        if (getLinkErrors.Count != 0)
            return new ErrorList(getLinkErrors);

        return Result.Success<IEnumerable<string>, ErrorList>(links);
    }

    private async Task<Result<bool, Error>> IsFileExists(
        string objectName,
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsResult = await IsBucketExists(bucketName, cancellationToken);

        if (bucketExistsResult.IsFailure)
            return bucketExistsResult;

        if (!bucketExistsResult.Value)
            return false;

        var objectExistsArgs = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        try
        {
            var objectStatInfo = await _minioClient.StatObjectAsync(objectExistsArgs, cancellationToken);

            return objectStatInfo.ObjectName != string.Empty;
        }
        catch
        {
            return Errors.General.UploadFailure(
                $"Failed to check if object '{objectName}' exists in bucket '{bucketName}'");
        }
    }

    private async Task<Result<bool, Error>> IsBucketExists(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        try
        {
            return await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        }
        catch
        {
            return Errors.General.UploadFailure($"Failed to check if bucket '{bucketName}' exists");
        }
    }

    private async Task<UnitResult<Error>> EnsureMakeBucket(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsResult = await IsBucketExists(bucketName, cancellationToken);

        if (bucketExistsResult.IsFailure)
            return bucketExistsResult;

        if (bucketExistsResult.Value)
            return UnitResult.Success<Error>();

        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(bucketName);

        try
        {
            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
        catch
        {
            return Errors.General.UploadFailure($"Failed to create bucket '{bucketName}'");
        }

        _logger.LogInformation("Successfully created bucket '{bucketName}'", bucketName);

        return UnitResult.Success<Error>();
    }
}