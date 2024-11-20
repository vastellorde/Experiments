namespace Experiments.UseCases.Profile.SetUserName;

public record SetUserNameCommand(string userName, int userId) : ICommand<Result>;
