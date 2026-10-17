using Microsoft.Extensions.Options;
using PawPaw.UseCases.Files;

namespace PawPaw.Infrastructure.Storage;

public sealed class LocalFileStorage(IOptions<LocalStorageOptions> options) : IFileStorage
{
  private readonly string _rootPath = Path.GetFullPath(options.Value.LocalPath);

  public async Task<StoredObject> SaveAsync(
    Stream content,
    string fileName,
    string contentType,
    CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(content);
    ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
    ArgumentException.ThrowIfNullOrWhiteSpace(contentType);

    var extension = Path.GetExtension(fileName).ToLowerInvariant();
    var storageKey = $"{DateTimeOffset.UtcNow:yyyy/MM}/{Guid.NewGuid():N}{extension}";
    var fullPath = ResolvePath(storageKey);

    Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

    await using var output = new FileStream(
      fullPath,
      FileMode.CreateNew,
      FileAccess.Write,
      FileShare.None,
      bufferSize: 81920,
      useAsync: true);

    await content.CopyToAsync(output, cancellationToken);
    await output.FlushAsync(cancellationToken);

    return new StoredObject(storageKey, output.Length, contentType);
  }

  public Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    var stream = new FileStream(
      ResolvePath(storageKey),
      FileMode.Open,
      FileAccess.Read,
      FileShare.Read,
      bufferSize: 81920,
      useAsync: true);

    return Task.FromResult<Stream>(stream);
  }

  public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    var fullPath = ResolvePath(storageKey);

    if (File.Exists(fullPath))
    {
      File.Delete(fullPath);
    }

    return Task.CompletedTask;
  }

  private string ResolvePath(string storageKey)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);

    var relativePath = storageKey.Replace('/', Path.DirectorySeparatorChar);
    var fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
    var rootWithSeparator = _rootPath.EndsWith(Path.DirectorySeparatorChar)
      ? _rootPath
      : _rootPath + Path.DirectorySeparatorChar;

    if (!fullPath.StartsWith(rootWithSeparator, StringComparison.Ordinal))
    {
      throw new InvalidOperationException("The storage key resolves outside the configured storage root.");
    }

    return fullPath;
  }
}
