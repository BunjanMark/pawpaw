using PawPaw.Core.Clinics;
using PawPaw.Core.Common;
using PawPaw.Core.Pets;
using PawPaw.Core.Veterinarians;

namespace PawPaw.Core.Appointments;

public enum AppointmentStatus
{
  Scheduled = 0,
  Cancelled = 1,
  Completed = 2,
  NoShow = 3
}

public sealed class Appointment : EntityBase
{
  public Guid PetId { get; set; }
  public Pet Pet { get; set; } = null!;
  public Guid ClinicId { get; set; }
  public Clinic Clinic { get; set; } = null!;
  public Guid? VeterinarianId { get; set; }
  public Veterinarian? Veterinarian { get; set; }
  public DateTimeOffset ScheduledAtUtc { get; set; }
  public string Reason { get; set; } = string.Empty;
  public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
}
