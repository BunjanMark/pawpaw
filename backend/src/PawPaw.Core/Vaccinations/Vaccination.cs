using PawPaw.Core.Common;
using PawPaw.Core.Pets;

namespace PawPaw.Core.Vaccinations;

public sealed class Vaccination : EntityBase
{
  public Guid PetId { get; set; }
  public Pet Pet { get; set; } = null!;
  public string VaccineName { get; set; } = string.Empty;
  public DateTimeOffset AdministeredAtUtc { get; set; }
  public DateTimeOffset? NextDueAtUtc { get; set; }
  public string? Notes { get; set; }
}
