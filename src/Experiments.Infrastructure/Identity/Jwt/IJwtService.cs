using Experiments.Infrastructure.Identity.Jwt.Models;
using Experiments.Infrastructure.Identity.Models;

namespace Experiments.Infrastructure.Identity.Jwt;

public interface IJwtService
{
  Task<(string token, DateTime expires)> GenerateJwtAsync(AppUser user);
  RefreshToken GenerateRefreshTokenAsync(AppUser user);
}
