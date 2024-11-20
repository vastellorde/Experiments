using Experiments.Infrastructure.Identity.Jwt.Models;

namespace Experiments.Infrastructure.Identity.Jwt.Config;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Token).IsRequired();
    builder.HasOne(x => x.User)
      .WithMany(x => x.RefreshTokens)
      .HasForeignKey(x => x.UserId);
  }
}
