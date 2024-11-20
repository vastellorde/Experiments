namespace Experiments.Core.SessionAggregate;

public class RefreshToken : EntityBase<Guid>, IAggregateRoot
{
  public Session Session { get; set; } = default!;
  public bool IsRevoked { get; set; }
  public int UserId { get; set; } = default!;

  private void SetRevoked()
  {
    IsRevoked = true;
  }
}
