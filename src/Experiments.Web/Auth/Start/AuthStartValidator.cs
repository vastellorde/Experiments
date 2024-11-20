using FluentValidation;

namespace Experiments.Web.Auth.Start;

public class AuthStartValidator : Validator<AuthStartRequest>
{
  public AuthStartValidator()
  {
    RuleFor(x => x.PhoneNumber)
      .NotEmpty()
      .WithMessage("Phone number is required");
  }
}
