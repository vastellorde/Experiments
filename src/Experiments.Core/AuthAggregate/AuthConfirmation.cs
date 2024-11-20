namespace Experiments.Core.AuthAggregate;

public class AuthConfirmation : EntityBase<Guid>, IAggregateRoot
{
  public AuthConfirmation()
  {
    Id = Guid.NewGuid();
  }

  public string PhoneNumber { get; set; } = default!;
  public string Code { get; set; } = default!;
  public bool IsConfirmed { get; set; }
  public string Platform { get; set; } = default!;

  public void SetConfirmed()
  {
    IsConfirmed = true;
  }
}
