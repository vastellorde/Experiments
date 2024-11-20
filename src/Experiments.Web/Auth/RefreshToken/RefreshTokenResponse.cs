namespace Experiments.Web.Auth.RefreshToken;

public class RefreshTokenResponse
{
  public string AccessToken { get; set; } = default!;
  public DateTimeOffset AccessTokenExpiry { get; set; }
  public string RefreshToken { get; set; } = default!;
  public DateTimeOffset RefreshTokenExpiry { get; set; }
}
