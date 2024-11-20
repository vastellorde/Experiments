using Experiments.Infrastructure.Chat;
using Experiments.Infrastructure.Identity.Jwt.Models;
using Microsoft.AspNetCore.Identity;

namespace Experiments.Infrastructure.Identity.Models;

public class AppUser : IdentityUser<int>
{
  public bool IsActive { get; set; } = true;
  public DateTimeOffset LastLogin { get; set; }
  public List<RefreshToken> RefreshTokens { get; set; } = [];
  public List<UserChat> UserChats { get; set; } = [];
  public List<Message> Messages { get; set; } = [];
}
