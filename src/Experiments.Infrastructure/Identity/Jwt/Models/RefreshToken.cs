using Experiments.Infrastructure.Identity.Models;

namespace Experiments.Infrastructure.Identity.Jwt.Models;

public class RefreshToken : EntityBase<Guid>, IAggregateRoot
{
  public string Token { get; set; } = default!;
  public DateTime Expires { get; set; }
  public bool IsRevoked { get; set; }
  public int UserId { get; set; }
  public AppUser User { get; set; } = default!;
  
  public void SetRevoked() => IsRevoked = true;
}
