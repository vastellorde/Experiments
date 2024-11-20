namespace Experiments.UseCases.Auth;

public class JwtDto
{
  public string AccessToken { get; set; } = string.Empty;
  public DateTimeOffset AccessTokenExpiry { get; set; }
  public string RefreshToken { get; set; } = string.Empty;
  public DateTimeOffset RefreshTokenExpiry { get; set; }
}
