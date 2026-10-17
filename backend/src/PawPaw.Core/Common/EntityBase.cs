namespace PawPaw.Core.Common;

public abstract class EntityBase
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedAtUtc { get; set; }
  public DateTimeOffset UpdatedAtUtc { get; set; }
}
