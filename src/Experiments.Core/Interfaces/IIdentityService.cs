using Experiments.Core.IdentityAggregate;

namespace Experiments.Core.Interfaces;

public interface IIdentityService
{
  Task<Result<(Jwt, JwtRefreshToken)>> SignInOrSignUpAsync(string phoneNumber);
  Task<Result> SetUserNameAsync(int userId, string userName);

  Task<Result<User>> GetUserAsync(int userId);
}
