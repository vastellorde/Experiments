namespace Experiments.Infrastructure.Chat.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
  public void Configure(EntityTypeBuilder<Message> builder)
  {
    builder.HasOne(x => x.Chat)
      .WithMany(x => x.Messages)
      .HasForeignKey(x => x.ChatId);
    builder.HasOne(x => x.Sender)
      .WithMany(x => x.Messages)
      .HasForeignKey(x => x.SenderId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
