using JobsityChallenge.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobsityChallenge.Infrastructure.Data.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable(nameof(ChatMessage));

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Content).IsRequired().HasColumnType("varchar(max)");

        builder.HasOne(m => m.User);
        builder.HasOne(m => m.ChatRoom);
    }
}
