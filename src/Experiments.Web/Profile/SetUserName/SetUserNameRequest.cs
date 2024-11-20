using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Experiments.Web.Profile.SetUserName;

public class SetUserNameRequest
{
  public const string Route = "/profile/set-username";

  [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
  public string UserId { get; set; } = default!;

  [Required] public string UserName { get; set; } = default!;
}
