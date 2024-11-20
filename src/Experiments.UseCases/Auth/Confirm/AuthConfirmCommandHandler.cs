using Experiments.Core.AuthAggregate;
using Experiments.Core.AuthAggregate.Specifications;
using Experiments.Core.Interfaces;

namespace Experiments.UseCases.Auth.Confirm;

public class AuthConfirmCommandHandler(
  IRepository<AuthConfirmation> repository,
  IIdentityService identityService
)
  : ICommandHandler<AuthConfirmCommand, Result<JwtDto>>
{
  public async Task<Result<JwtDto>> Handle(AuthConfirmCommand request, CancellationToken cancellationToken)
  {
    var spec = new AuthConfirmationById(Guid.Parse(request.confirmationId));
    var confirmation = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (confirmation == null)
    {
      return Result.NotFound();
    }

    if (confirmation.IsConfirmed)
    {
      return Result.Forbidden();
    }

    if (confirmation.Code != request.code)
    {
      return Result.Invalid();
    }

    var tokensResult = await identityService.SignInOrSignUpAsync(confirmation.PhoneNumber);

    if (!tokensResult.IsSuccess)
    {
      return Result.Unauthorized();
    }

    confirmation.SetConfirmed();
    await repository.UpdateAsync(confirmation, cancellationToken);

    return Result.Success(new JwtDto
    {
      AccessToken = tokensResult.Value.AccessToken,
      RefreshToken = tokensResult.Value.RefreshToken,
      AccessTokenExpiry = tokensResult.Value.AccessTokenExpiry,
      RefreshTokenExpiry = tokensResult.Value.RefreshTokenExpiry
    });
  }
}
