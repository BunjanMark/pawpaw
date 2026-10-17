namespace PawPaw.Infrastructure.Storage;

public sealed class LocalStorageOptions
{
  public const string SectionName = "Storage";
  public string Provider { get; init; } = "Local";
  public string LocalPath { get; init; } = "../../storage";
}
