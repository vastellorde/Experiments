using Experiments.Core.IdentityAggregate;
using Experiments.Infrastructure.Identity.Models;

namespace Experiments.Infrastructure.Identity.Jwt;

public interface IJwtService
{
  Task<(Core.IdentityAggregate.Jwt, JwtRefreshToken)> GenerateJwtAsync(AppUser user);
}
