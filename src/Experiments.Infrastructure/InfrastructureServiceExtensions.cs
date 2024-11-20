using System.Text;
using Experiments.Core.Interfaces;
using Experiments.Core.Services;
using Experiments.Infrastructure.Data;
using Experiments.Infrastructure.Data.Queries;
using Experiments.Infrastructure.Identity;
using Experiments.Infrastructure.Identity.Jwt;
using Experiments.Infrastructure.Identity.Models;
using Experiments.UseCases.Contributors.List;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Experiments.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    var connectionString = config.GetConnectionString("PostgresConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(connectionString));

    services.AddScoped<IUserValidator<AppUser>, CustomUserValidator>()
      .AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimPrincipalFactory>();

    services.AddIdentity<AppUser, AppRole>()
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
      .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
      .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
      .AddScoped<IDeleteContributorService, DeleteContributorService>()
      .AddScoped<IIdentityService, IdentityService>()
      .AddScoped<IJwtService, JwtService>();

    services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    services.AddAuthorization();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
