namespace Experiments.Core.IdentityAggregate;

public class JwtRefreshToken
{
  public string Token { get; set; } = default!;
  public Guid Id { get; set; }
  public DateTime Expires { get; set; }
  public int UserId { get; set; }
}
