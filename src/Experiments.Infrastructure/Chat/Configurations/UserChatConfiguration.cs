namespace Experiments.Infrastructure.Chat.Configurations;

public class UserChatConfiguration : IEntityTypeConfiguration<UserChat>
{
  public void Configure(EntityTypeBuilder<UserChat> builder)
  {
    builder.HasKey(x => new { x.UserId, x.ChatId });

    builder.HasOne(x => x.User)
      .WithMany(x => x.UserChats)
      .HasForeignKey(x => x.UserId);

    builder.HasOne(x => x.Chat)
      .WithMany(x => x.Participants)
      .HasForeignKey(x => x.ChatId);
  }
}
