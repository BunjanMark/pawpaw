using PawPaw.Core.Common;
using PawPaw.Core.Users;

namespace PawPaw.Core.Notifications;

public enum NotificationKind
{
  General = 0,
  Appointment = 1,
  Vaccination = 2
}

public sealed class Notification : EntityBase
{
  public Guid UserAccountId { get; set; }
  public UserAccount UserAccount { get; set; } = null!;
  public NotificationKind Kind { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Body { get; set; } = string.Empty;
  public bool IsRead { get; set; }
  public DateTimeOffset? ReadAtUtc { get; set; }
}
