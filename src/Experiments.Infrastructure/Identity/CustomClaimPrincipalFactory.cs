using System.Security.Claims;
using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Experiments.Infrastructure.Identity;

public class CustomClaimPrincipalFactory(
  IOptions<IdentityOptions> options,
  UserManager<AppUser> userManager) : IUserClaimsPrincipalFactory<AppUser>
{
  private IdentityOptions Options { get; } = options.Value;

  public async Task<ClaimsPrincipal> CreateAsync(AppUser user)
  {
    var claims = await GenerateClaimsAsync(user);

    return new ClaimsPrincipal(claims);
  }

  private async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
  {
    var userId = user.Id.ToString();
    var claims = new ClaimsIdentity(
      IdentityConstants.ApplicationScheme,
      Options.ClaimsIdentity.UserNameClaimType,
      Options.ClaimsIdentity.RoleClaimType);

    claims.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
    if (user.UserName is not null)
    {
      claims.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, user.UserName));
    }

    if (userManager.SupportsUserSecurityStamp)
    {
      claims.AddClaim(new Claim(
        Options.ClaimsIdentity.SecurityStampClaimType,
        await userManager.GetSecurityStampAsync(user)));
    }

    if (userManager.SupportsUserClaim)
    {
      claims.AddClaims(await userManager.GetClaimsAsync(user));
    }

    // Add more as needed

    return claims;
  }
}
