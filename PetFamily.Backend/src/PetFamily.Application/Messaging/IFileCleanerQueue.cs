namespace PetFamily.Application.Messaging;

public interface IFileCleanerQueue
{
    Task PublishAsync(IEnumerable<string> files, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> ConsumeAsync(CancellationToken cancellationToken = default);
}