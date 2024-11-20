namespace Experiments.Web.Auth.Confirm;

public class AuthConfirmResponse
{
  public string AccessToken { get; set; } = default!;
  public DateTimeOffset AccessTokenExpiry { get; set; }
  public string RefreshToken { get; set; } = default!;
  public DateTimeOffset RefreshTokenExpiry { get; set; }
}
