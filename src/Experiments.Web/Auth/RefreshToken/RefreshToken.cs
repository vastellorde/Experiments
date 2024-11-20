namespace Experiments.Web.Auth.RefreshToken;

public class RefreshToken : Endpoint<RefreshTokenRequest, RefreshTokenResponse>
{
  public override void Configure()
  {
    Post(RefreshTokenRequest.Route);
  }

  public override Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
  {
    return base.HandleAsync(req, ct);
  }
}
