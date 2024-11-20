using Experiments.Core.AuthAggregate;
using Experiments.Core.ContributorAggregate;
using Experiments.Core.SessionAggregate;
using Experiments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Experiments.Infrastructure.Data;

public class AppDbContext(
  DbContextOptions<AppDbContext> options,
  IDomainEventDispatcher? dispatcher) : IdentityDbContext<AppUser, AppRole, int>(options)
{
  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<AuthConfirmation> AuthConfirmations => Set<AuthConfirmation>();
  public DbSet<Session> Sessions => Set<Session>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    modelBuilder.Entity<RefreshToken>()
      .HasOne(x => x.Session)
      .WithOne(x => x.refreshToken)
      .HasForeignKey<Session>(x => x.refreshTokenId)
      .IsRequired();
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
  {
    var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (dispatcher == null)
    {
      return result;
    }

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<HasDomainEventsBase>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Count != 0)
      .ToArray();

    await dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
