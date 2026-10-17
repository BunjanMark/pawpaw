using PawPaw.Core.Common;
using PawPaw.Core.Pets;

namespace PawPaw.Core.Users;

public sealed class UserAccount : EntityBase
{
  public string Email { get; set; } = string.Empty;
  public string DisplayName { get; set; } = string.Empty;
  public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
