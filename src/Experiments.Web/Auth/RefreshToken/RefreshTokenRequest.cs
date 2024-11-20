using System.ComponentModel.DataAnnotations;

namespace Experiments.Web.Auth.RefreshToken;

public class RefreshTokenRequest
{
  public const string Route = "/auth/refresh-token";

  [Required] public string RefreshToken { get; set; } = default!;
}
