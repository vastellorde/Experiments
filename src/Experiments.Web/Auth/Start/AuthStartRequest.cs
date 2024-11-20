using System.ComponentModel.DataAnnotations;

namespace Experiments.Web.Auth.Start;

public class AuthStartRequest
{
  public const string Route = "/auth/start";

  [Required] public string PhoneNumber { get; set; } = default!;
  [Required] public string Platform { get; set; } = "Unknown";
}
