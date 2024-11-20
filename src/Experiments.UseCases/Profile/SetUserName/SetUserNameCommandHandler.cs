using Experiments.Core.Interfaces;

namespace Experiments.UseCases.Profile.SetUserName;

public class SetUserNameCommandHandler(IIdentityService identityService) : ICommandHandler<SetUserNameCommand, Result>
{
  public Task<Result> Handle(SetUserNameCommand request, CancellationToken cancellationToken)
  {
    return identityService.SetUserNameAsync(request.userId, request.userName);
  }
}
