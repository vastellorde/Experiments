using Experiments.UseCases.Profile.Get;

namespace Experiments.Web.Profile.Get;

public class GetProfile(IMediator mediator) : Endpoint<GetProfileRequest, GetProfileResponse>
{
  public override void Configure()
  {
    Get(GetProfileRequest.Route);
  }

  public override async Task HandleAsync(GetProfileRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new GetProfileCommand(req.Id), ct);

    if (result.IsNotFound())
    {
      AddError(x => x.Id, "Profile not found");
      ThrowIfAnyErrors();
    }

    Response = new GetProfileResponse
    {
      Id = result.Value.Id,
      UserName = result.Value.UserName,
      PhoneNumber = result.Value.PhoneNumber,
      LastLoginAt = result.Value.LastLoginAt
    };
  }
}
