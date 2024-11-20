using FluentValidation;

namespace Experiments.Web.Auth.Confirm;

public class AuthConfirmValidator : Validator<AuthConfirmRequest>
{
  public AuthConfirmValidator()
  {
    RuleFor(x => x.ConfirmationId)
      .NotEmpty()
      .WithMessage("Please specify a valid confirmation id");

    RuleFor(x => x.Code)
      .NotEmpty()
      .WithMessage("Please specify a valid code")
      .Length(4, 4)
      .WithMessage("Code must be 4 characters");
  }
}
