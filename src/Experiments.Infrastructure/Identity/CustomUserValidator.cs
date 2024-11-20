using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Experiments.Infrastructure.Identity;

public class CustomUserValidator : UserValidator<AppUser>
{
  public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
  {
    var result = await base.ValidateAsync(manager, user);
    if (result.Succeeded)
    {
      return result;
    }

    var errors = result.Errors.ToList();
    if (user.UserName is null)
    {
      var expectedError = Describer.InvalidUserName(user.UserName);
      errors.RemoveAll(x => x.Code == expectedError.Code);
    }

    if (user.Email is null)
    {
      var expectedError = Describer.InvalidEmail(user.Email);
      errors.RemoveAll(x => x.Code == expectedError.Code);
    }

    return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
  }
}
