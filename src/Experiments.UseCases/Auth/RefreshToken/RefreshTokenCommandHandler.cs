using Experiments.Core.Interfaces;

namespace Experiments.UseCases.Auth.RefreshToken;

public class RefreshTokenCommandHandler(IIdentityService identityService)
  : ICommandHandler<RefreshTokenCommand, Result<JwtDto>>
{
  public async Task<Result<JwtDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
  {
    var result = await identityService.RefreshTokenAsync(request.RefreshToken);

    if (result.IsNotFound())
    {
      return Result.NotFound();
    }

    if (result.IsForbidden())
    {
      return Result.Forbidden();
    }

    return Result.Success(new JwtDto
    {
      AccessToken = result.Value.AccessToken,
      RefreshToken = result.Value.RefreshToken,
      AccessTokenExpiry = result.Value.AccessTokenExpiry,
      RefreshTokenExpiry = result.Value.RefreshTokenExpiry
    });
  }
}
