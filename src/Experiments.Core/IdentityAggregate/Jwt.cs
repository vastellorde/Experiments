namespace Experiments.Core.IdentityAggregate;

public class Jwt
{
  public string AccessToken { get; set; } = string.Empty;
  public DateTimeOffset AccessTokenExpiry { get; set; }
}
