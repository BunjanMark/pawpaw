using PawPaw.Core.Appointments;
using PawPaw.Core.Clinics;
using PawPaw.Core.Common;

namespace PawPaw.Core.Veterinarians;

public sealed class Veterinarian : EntityBase
{
  public Guid ClinicId { get; set; }
  public Clinic Clinic { get; set; } = null!;
  public string FullName { get; set; } = string.Empty;
  public string? LicenseNumber { get; set; }
  public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
