namespace Experiments.Core.AuthAggregate.Specifications;

public sealed class AuthConfirmationById : Specification<AuthConfirmation>
{
  public AuthConfirmationById(Guid id)
  {
    Query.Where(a => a.Id == id);
  }
}
