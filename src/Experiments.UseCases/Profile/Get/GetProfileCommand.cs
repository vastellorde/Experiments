namespace Experiments.UseCases.Profile.Get;

public record GetProfileCommand(int userId) : ICommand<Result<UserDto>>;
