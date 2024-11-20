namespace Experiments.UseCases.Auth.Confirm;

public record AuthConfirmCommand(string confirmationId, string code) : ICommand<Result<JwtDto>>;
