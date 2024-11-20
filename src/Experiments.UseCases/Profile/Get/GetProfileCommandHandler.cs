using Experiments.Core.Interfaces;

namespace Experiments.UseCases.Profile.Get;

public class GetProfileCommandHandler(IIdentityService identityService)
  : ICommandHandler<GetProfileCommand, Result<UserDto>>
{
  public async Task<Result<UserDto>> Handle(GetProfileCommand request, CancellationToken cancellationToken)
  {
    var user = await identityService.GetUserAsync(request.userId);

    if (user.IsSuccess)
    {
      return Result.Success(new UserDto
      {
        Id = user.Value.Id,
        PhoneNumber = user.Value.PhoneNumber,
        UserName = user.Value.UserName,
        LastLoginAt = user.Value.LastLogin
      });
    }

    return Result.NotFound();
  }
}
