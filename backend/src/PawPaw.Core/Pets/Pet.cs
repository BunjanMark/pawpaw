using PawPaw.Core.Appointments;
using PawPaw.Core.Common;
using PawPaw.Core.Files;
using PawPaw.Core.Users;
using PawPaw.Core.Vaccinations;

namespace PawPaw.Core.Pets;

public sealed class Pet : EntityBase
{
  public Guid UserAccountId { get; set; }
  public UserAccount Owner { get; set; } = null!;
  public string Name { get; set; } = string.Empty;
  public string Species { get; set; } = string.Empty;
  public string? Breed { get; set; }
  public DateOnly? DateOfBirth { get; set; }
  public PetProfile? Profile { get; set; }
  public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
  public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
  public ICollection<FileAsset> Files { get; set; } = new List<FileAsset>();
}
