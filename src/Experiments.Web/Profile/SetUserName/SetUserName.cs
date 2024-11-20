using Experiments.UseCases.Profile.SetUserName;

namespace Experiments.Web.Profile.SetUserName;

public class SetUserName(IMediator mediator) : Endpoint<SetUserNameRequest>
{
  public override void Configure()
  {
    Post(SetUserNameRequest.Route);
  }

  public override async Task HandleAsync(SetUserNameRequest req, CancellationToken ct)
  {
    var userId = Convert.ToInt32(req.UserId);
    var result = await mediator.Send(new SetUserNameCommand(req.UserName, userId), ct);

    if (result.IsNotFound())
    {
      AddError("User not found");
    }

    if (result.IsError())
    {
      AddError("cant set user name");
    }

    if (result.IsSuccess)
    {
      await SendOkAsync(ct);
    }

    ThrowIfAnyErrors();
  }
}
