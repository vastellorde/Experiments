namespace Experiments.Infrastructure.Chat;

public class ChatRoom : EntityBase<Guid>, IAggregateRoot
{
  public string? Name { get; set; }
  public ICollection<UserChat> Participants { get; set; } = [];
  public ICollection<Message> Messages { get; set; } = [];
  public bool IsGroup { get; set; }
}
