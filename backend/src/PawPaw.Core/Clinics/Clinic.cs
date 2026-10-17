using PawPaw.Core.Common;
using PawPaw.Core.Veterinarians;

namespace PawPaw.Core.Clinics;

public sealed class Clinic : EntityBase
{
  public string Name { get; set; } = string.Empty;
  public string? Address { get; set; }
  public string? PhoneNumber { get; set; }
  public ICollection<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();
}
