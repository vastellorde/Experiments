using Ardalis.SharedKernel;
using Experiments.Core.ContributorAggregate;
using Experiments.UseCases.Contributors.Create;
using MediatR;
using System.Reflection;

namespace Experiments.Web.Configurations;

public static class MediatrConfigs
{
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        var mediatRAssemblies = new[]
        {
            Assembly.GetAssembly(typeof(Contributor)), // Core
            Assembly.GetAssembly(typeof(CreateContributorCommand)) // UseCases
        };

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        return services;
    }
}