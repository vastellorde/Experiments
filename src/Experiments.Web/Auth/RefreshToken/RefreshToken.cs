using Experiments.UseCases.Auth.RefreshToken;

namespace Experiments.Web.Auth.RefreshToken;

public class RefreshToken(IMediator mediator) : Endpoint<RefreshTokenRequest, RefreshTokenResponse>
{
  public override void Configure()
  {
    Post(RefreshTokenRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new RefreshTokenCommand(req.RefreshToken), ct);

    if (result.IsNotFound())
    {
      AddError(x => x.RefreshToken, "Refresh token not found");
    }

    if (result.IsForbidden())
    {
      AddError(x => x.RefreshToken, "Refresh token is revoked");
    }

    ThrowIfAnyErrors(StatusCodes.Status401Unauthorized);

    Response = new RefreshTokenResponse
    {
      AccessToken = result.Value.AccessToken,
      RefreshToken = result.Value.RefreshToken,
      AccessTokenExpiry = result.Value.AccessTokenExpiry,
      RefreshTokenExpiry = result.Value.RefreshTokenExpiry
    };
  }
}
