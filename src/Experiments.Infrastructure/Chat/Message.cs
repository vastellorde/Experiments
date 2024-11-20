using Experiments.Infrastructure.Identity.Models;

namespace Experiments.Infrastructure.Chat;

public class Message : EntityBase<Guid>, IAggregateRoot
{
  public Guid ChatId { get; set; }
  public ChatRoom Chat { get; set; } = default!;
  public int SenderId { get; set; }
  public AppUser Sender { get; set; } = default!;
  public string Text { get; set; } = default!;
  public DateTime Timestamp { get; set; }
  public bool IsRead { get; set; }
}
