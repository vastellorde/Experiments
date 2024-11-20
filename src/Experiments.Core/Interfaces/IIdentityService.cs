using Experiments.Core.IdentityAggregate;

namespace Experiments.Core.Interfaces;

public interface IIdentityService
{
  Task<Result<Jwt>> SignInOrSignUpAsync(string phoneNumber);
  Task<Result<Jwt>> RefreshTokenAsync(string refreshToken);
  Task<Result> SetUserNameAsync(int userId, string userName);

  Task<Result<User>> GetUserAsync(int userId);
}
