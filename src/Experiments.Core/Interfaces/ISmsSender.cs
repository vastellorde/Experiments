namespace Experiments.Core.Interfaces;

public interface ISmsSender
{
  Task SendCodeAsync(string phoneNumber, string code);
}
