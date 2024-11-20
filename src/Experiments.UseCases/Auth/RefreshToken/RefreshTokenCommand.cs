namespace Experiments.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<Result<JwtDto>>;
