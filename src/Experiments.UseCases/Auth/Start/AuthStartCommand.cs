namespace Experiments.UseCases.Auth.Start;

public record AuthStartCommand(string phoneNumber, string platform) : ICommand<Result<string>>;
