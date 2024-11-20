using Experiments.UseCases.Auth.Confirm;

namespace Experiments.Web.Auth.Confirm;

public class AuthConfirm(IMediator mediator) : Endpoint<AuthConfirmRequest, AuthConfirmResponse>
{
  public override void Configure()
  {
    Post(AuthConfirmRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(AuthConfirmRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new AuthConfirmCommand(req.ConfirmationId, req.Code), ct);

    if (result.IsNotFound())
    {
      AddError(x => x.ConfirmationId, "AuthConfirmation not found");
      ThrowIfAnyErrors();
    }

    if (result.IsForbidden())
    {
      AddError(x => x.ConfirmationId, "AuthConfirmation already confirmed");
      ThrowIfAnyErrors();
    }

    if (result.IsInvalid())
    {
      AddError(x => x.Code, "Verification code is invalid");
      ThrowIfAnyErrors();
    }

    if (result.IsUnauthorized())
    {
      AddError("Unknown error");
      ThrowIfAnyErrors();
    }

    Response = new AuthConfirmResponse
    {
      AccessToken = result.Value.AccessToken,
      RefreshToken = result.Value.RefreshToken,
      AccessTokenExpiry = result.Value.AccessTokenExpiry,
      RefreshTokenExpiry = result.Value.RefreshTokenExpiry
    };
  }
}
