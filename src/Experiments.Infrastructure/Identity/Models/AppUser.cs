using Microsoft.AspNetCore.Identity;

namespace Experiments.Infrastructure.Identity.Models;

public class AppUser : IdentityUser<int>
{
  public bool IsActive { get; set; } = true;
  public DateTimeOffset LastLogin { get; set; }
}
