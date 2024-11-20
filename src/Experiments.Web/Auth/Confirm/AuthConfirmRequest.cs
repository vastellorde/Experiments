using System.ComponentModel.DataAnnotations;

namespace Experiments.Web.Auth.Confirm;

public class AuthConfirmRequest
{
  public const string Route = "/auth/confirm";

  [Required] public string ConfirmationId { get; set; } = default!;

  [Required] public string Code { get; set; } = default!;
}
