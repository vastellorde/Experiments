using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Experiments.Core.IdentityAggregate;
using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Experiments.Infrastructure.Identity.Jwt;

public class JwtService(
  IConfiguration configuration,
  UserManager<AppUser> userManager) : IJwtService
{
  public async Task<(Core.IdentityAggregate.Jwt, JwtRefreshToken)> GenerateJwtAsync(AppUser user)
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
    var refreshToken = GenerateRefreshToken(user.Id);
    return (
      new Core.IdentityAggregate.Jwt
      {
        AccessToken = tokenHandler.WriteToken(securityToken), AccessTokenExpiry = securityToken.ValidTo
      }, refreshToken);
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

  private JwtRefreshToken GenerateRefreshToken(int userId)
  {
    var secretKey = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);
    var signingCredentials =
      new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
    var tokenHandler = new JwtSecurityTokenHandler();

    var tokenId = Guid.NewGuid();

    var descriptor = new SecurityTokenDescriptor
    {
      Issuer = configuration["Jwt:Issuer"],
      Audience = configuration["Jwt:Audience"],
      IssuedAt = DateTime.UtcNow,
      Expires = DateTime.UtcNow.AddDays(7),
      Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, tokenId.ToString())]),
      SigningCredentials = signingCredentials
    };

    var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

    return new JwtRefreshToken
    {
      Token = tokenHandler.WriteToken(securityToken), Expires = securityToken.ValidTo, Id = tokenId, UserId = userId
    };
  }
}
