using FluentValidation;

namespace Experiments.Web.Profile.SetUserName;

public class SetUserNameValidator : Validator<SetUserNameRequest>
{
  public SetUserNameValidator()
  {
    RuleFor(x => x.UserName)
      .NotEmpty()
      .WithMessage("Please specify a username");
  }
}
