using Experiments.Core.AuthAggregate;
using Experiments.Core.AuthAggregate.Specifications;
using Experiments.Core.Interfaces;
using Experiments.Core.SessionAggregate;

namespace Experiments.UseCases.Auth.Confirm;

public class AuthConfirmCommandHandler(
  IRepository<AuthConfirmation> repository,
  IIdentityService identityService,
  IRepository<Core.SessionAggregate.RefreshToken> refreshTokenRepository
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

    var refreshToken = new Core.SessionAggregate.RefreshToken
    {
      Id = tokensResult.Value.Item2.Id,
      Session = new Session { Platform = confirmation.Platform.ToSessionPlatform(), CreatedAt = DateTime.UtcNow },
      UserId = tokensResult.Value.Item2.UserId
    };

    await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

    return Result.Success(new JwtDto
    {
      AccessToken = tokensResult.Value.Item1.AccessToken,
      RefreshToken = tokensResult.Value.Item2.Token,
      AccessTokenExpiry = tokensResult.Value.Item1.AccessTokenExpiry,
      RefreshTokenExpiry = tokensResult.Value.Item2.Expires
    });
  }
}
