using Ardalis.Result;
using Experiments.Core.IdentityAggregate;
using Experiments.Core.Interfaces;
using Experiments.Infrastructure.Data;
using Experiments.Infrastructure.Identity.Jwt;
using Experiments.Infrastructure.Identity.Jwt.Models;
using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Experiments.Infrastructure.Identity;

public class IdentityService(
  UserManager<AppUser> userManager,
  IJwtService jwtService,
  AppDbContext db,
  IRepository<RefreshToken> repository) : IIdentityService
{
  public async Task<Result<Core.IdentityAggregate.Jwt>> RefreshTokenAsync(string refreshToken)
  {
    var token = db.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

    if (token == null)
    {
      return Result.NotFound();
    }

    if (token.IsRevoked)
    {
      return Result.Forbidden();
    }

    token.SetRevoked();

    await repository.UpdateAsync(token);

    var user = await userManager.FindByIdAsync(token.UserId.ToString());

    var newToken = await jwtService.GenerateJwtAsync(user!);
    var newRefreshToken = jwtService.GenerateRefreshTokenAsync(user!);

    await repository.AddAsync(newRefreshToken);

    return Result.Success(new Core.IdentityAggregate.Jwt
    {
      AccessToken = newToken.token,
      RefreshToken = newRefreshToken.Token,
      AccessTokenExpiry = newToken.expires,
      RefreshTokenExpiry = newRefreshToken.Expires
    });
  }

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

  public async Task<Result<Core.IdentityAggregate.Jwt>> SignInOrSignUpAsync(string phoneNumber)
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

    var token = await jwtService.GenerateJwtAsync(user);
    var refreshToken = jwtService.GenerateRefreshTokenAsync(user);

    await repository.AddAsync(refreshToken);

    return Result.Success(new Core.IdentityAggregate.Jwt
    {
      AccessToken = token.token,
      RefreshToken = refreshToken.Token,
      AccessTokenExpiry = token.expires,
      RefreshTokenExpiry = refreshToken.Expires
    });
  }
}
