using JobsityChallenge.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobsityChallenge.Infrastructure.Data.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.ToTable(nameof(ChatRoom));

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(256)");

        builder.HasMany(cr => cr.Users)
               .WithMany() 
               .UsingEntity<Dictionary<string, object>>(
                    "ChatRoomUsers", 
                    j => j.HasOne<IdentityUser>()
                          .WithMany()
                          .HasForeignKey("UserId")
                          .HasPrincipalKey(u => u.Id)
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<ChatRoom>()
                          .WithMany()
                          .HasForeignKey("ChatRoomId")
                          .HasPrincipalKey(cr => cr.Id)
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("ChatRoomId", "UserId");
                        j.ToTable("ChatRoomUsers");
                    });
    }
}
