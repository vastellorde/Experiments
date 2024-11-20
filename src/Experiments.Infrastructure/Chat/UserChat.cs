using Experiments.Infrastructure.Identity.Models;

namespace Experiments.Infrastructure.Chat;

public class UserChat
{
  public int UserId { get; set; }
  public AppUser User { get; set; } = default!;
  public Guid ChatId { get; set; }
  public ChatRoom Chat { get; set; } = default!;
}
