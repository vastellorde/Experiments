using Experiments.Core.Interfaces;
using Experiments.Infrastructure;
using Experiments.Infrastructure.Email;
using Experiments.Infrastructure.Sms;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Experiments.Web.Configurations;

public static class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services,
    ILogger logger, WebApplicationBuilder builder)
  {
    services.AddInfrastructureServices(builder.Configuration, logger)
      .AddMediatrConfigs();


    if (builder.Environment.IsDevelopment())
    {
      // Use a local test email server
      // See: https://ardalis.com/configuring-a-local-test-email-server/
      services.AddScoped<IEmailSender, MimeKitEmailSender>();

      // Otherwise use this:
      //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
    }
    else
    {
      services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }

    services.AddScoped<ISmsSender, SmsSender>();

    logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

    return services;
  }
}
