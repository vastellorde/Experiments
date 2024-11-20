using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Experiments.Infrastructure.Identity.Jwt.Models;
using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Experiments.Infrastructure.Identity.Jwt;

public class JwtService(
  IConfiguration configuration,
  UserManager<AppUser> userManager) : IJwtService
{
  public async Task<(string token, DateTime expires)> GenerateJwtAsync(AppUser user)
  {
    var secretKey = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);
    var signingCredentials =
      new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
    var tokenHandler = new JwtSecurityTokenHandler();
    var claims = await GetClaimsAsync(user);

    var descriptor = new SecurityTokenDescriptor
    {
      Issuer = configuration["Jwt:Issuer"],
      Audience = configuration["Jwt:Audience"],
      IssuedAt = DateTime.UtcNow,
      NotBefore = DateTime.UtcNow.AddMinutes(-5),
      Expires = DateTime.UtcNow.AddMinutes(5),
      SigningCredentials = signingCredentials,
      Subject = new ClaimsIdentity(claims)
    };

    var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
    return (tokenHandler.WriteToken(securityToken), securityToken.ValidTo);
  }

  public RefreshToken GenerateRefreshTokenAsync(AppUser user)
  {
    var secretKey = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);
    var signingCredentials =
      new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
    var tokenHandler = new JwtSecurityTokenHandler();

    var descriptor = new SecurityTokenDescriptor
    {
      Issuer = configuration["Jwt:Issuer"],
      Audience = configuration["Jwt:Audience"],
      IssuedAt = DateTime.UtcNow,
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = signingCredentials
    };

    var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

    return new RefreshToken
    {
      Token = tokenHandler.WriteToken(securityToken), 
      Expires = securityToken.ValidTo,
      UserId = user.Id,
    };
  }

  private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
  {
    var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()) };
    if (user.UserName != null)
    {
      claims.Add(new Claim(ClaimTypes.Name, user.UserName));
    }

    var userRoles = await userManager.GetRolesAsync(user);
    claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

    return claims;
  }
}
