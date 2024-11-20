using Experiments.Core.Interfaces;

namespace Experiments.Infrastructure.Sms;

public class SmsSender : ISmsSender
{
  public Task SendCodeAsync(string phoneNumber, string code)
  {
    return Task.CompletedTask;
  }
}
