using Ardalis.Result;
using Experiments.Core.IdentityAggregate;
using Experiments.Core.Interfaces;
using Experiments.Infrastructure.Identity.Jwt;
using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Experiments.Infrastructure.Identity;

public class IdentityService(
  UserManager<AppUser> userManager,
  IJwtService jwtService) : IIdentityService
{
  public async Task<Result> SetUserNameAsync(int userId, string userName)
  {
    var user = await userManager.FindByIdAsync(userId.ToString());

    if (user == null)
    {
      return Result.NotFound();
    }

    user.UserName = userName;

    var result = await userManager.UpdateAsync(user);

    return result == IdentityResult.Success ? Result.Success() : Result.Error();
  }

  public async Task<Result<User>> GetUserAsync(int userId)
  {
    var user = await userManager.FindByIdAsync(userId.ToString());

    if (user == null)
    {
      return Result.NotFound();
    }

    return Result.Success(new User
    {
      Id = user.Id, UserName = user.UserName, PhoneNumber = user.PhoneNumber ?? "", LastLogin = user.LastLogin
    });
  }

  public async Task<Result<(Core.IdentityAggregate.Jwt, JwtRefreshToken)>> SignInOrSignUpAsync(string phoneNumber)
  {
    var containsUser = await userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);

    AppUser? user = null;

    if (!containsUser)
    {
      var newUser = new AppUser { PhoneNumber = phoneNumber };

      var result = await userManager.CreateAsync(newUser);

      if (result.Succeeded)
      {
        user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
      }
    }
    else
    {
      user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    if (user == null)
    {
      return Result.Error();
    }

    var tokens = await jwtService.GenerateJwtAsync(user);

    return Result.Success(tokens);
  }
}
