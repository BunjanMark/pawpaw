using PawPaw.Core.Common;
using PawPaw.Core.Pets;
using PawPaw.Core.Users;

namespace PawPaw.Core.Files;

public sealed class FileAsset : EntityBase
{
  public Guid? UserAccountId { get; set; }
  public UserAccount? UserAccount { get; set; }
  public Guid? PetId { get; set; }
  public Pet? Pet { get; set; }
  public string StorageKey { get; set; } = string.Empty;
  public string OriginalFileName { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public long SizeBytes { get; set; }
}
