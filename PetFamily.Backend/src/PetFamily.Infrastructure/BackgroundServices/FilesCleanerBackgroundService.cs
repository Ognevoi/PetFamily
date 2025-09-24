using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IFileCleanerQueue _fileCleanerQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FilesCleanerBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IFileCleanerQueue fileCleanerQueue,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _fileCleanerQueue = fileCleanerQueue;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var filesProvider = scope.ServiceProvider.GetRequiredService<IFilesProvider>();

            var filesToDelete = await _fileCleanerQueue.ConsumeAsync(stoppingToken);

            _logger.LogInformation("Deleting {FileCount} files", filesToDelete.Count());

            await filesProvider.DeleteFiles(filesToDelete, Constants.BUCKET_NAME, stoppingToken);
        }

        _logger.LogInformation("FilesCleanerBackgroundService is stopping.");
    }
}