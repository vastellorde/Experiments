namespace Experiments.Core.IdentityAggregate;

public class User : EntityBase
{
  public bool IsActive { get; set; } = true;
  public DateTimeOffset LastLogin { get; set; }
  public string? UserName { get; set; }
  public string PhoneNumber { get; set; } = default!;
}
