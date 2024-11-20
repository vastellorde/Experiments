using Experiments.UseCases.Auth.Start;

namespace Experiments.Web.Auth.Start;

public class AuthStart(IMediator mediator) : Endpoint<AuthStartRequest, AuthStartResponse>
{
  public override void Configure()
  {
    Post(AuthStartRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(AuthStartRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new AuthStartCommand(req.PhoneNumber, req.Platform), ct);

    if (result.IsSuccess)
    {
      Response = new AuthStartResponse(result.Value);
      return;
    }

    ThrowIfAnyErrors();
  }
}
