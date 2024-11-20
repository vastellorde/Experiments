using Experiments.Core.AuthAggregate;
using Experiments.Core.Interfaces;

namespace Experiments.UseCases.Auth.Start;

public class AuthStartCommandHandler(IRepository<AuthConfirmation> repository, ISmsSender smsSender)
  : ICommandHandler<AuthStartCommand, Result<string>>
{
  public async Task<Result<string>> Handle(AuthStartCommand request, CancellationToken cancellationToken)
  {
    var code = new Random().Next(1000, 9999).ToString();
    await smsSender.SendCodeAsync(request.phoneNumber, code);

    var newConfirmation =
      new AuthConfirmation { Code = code, PhoneNumber = request.phoneNumber, Platform = request.platform };

    var confirmation = await repository.AddAsync(newConfirmation, cancellationToken);

    return Result.Success(confirmation.Id.ToString());
  }
}
