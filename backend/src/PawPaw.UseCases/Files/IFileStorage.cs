namespace PawPaw.UseCases.Files;

public sealed record StoredObject(string StorageKey, long SizeBytes, string ContentType);

public interface IFileStorage
{
  Task<StoredObject> SaveAsync(
    Stream content,
    string fileName,
    string contentType,
    CancellationToken cancellationToken = default);

  Task<Stream> OpenReadAsync(
    string storageKey,
    CancellationToken cancellationToken = default);

  Task DeleteAsync(
    string storageKey,
    CancellationToken cancellationToken = default);
}
