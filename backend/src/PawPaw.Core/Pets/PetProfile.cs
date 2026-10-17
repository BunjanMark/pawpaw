using PawPaw.Core.Common;

namespace PawPaw.Core.Pets;

public sealed class PetProfile : EntityBase
{
  public Guid PetId { get; set; }
  public Pet Pet { get; set; } = null!;
  public decimal? WeightKg { get; set; }
  public string? Biography { get; set; }
  public string? MedicalNotes { get; set; }
}
