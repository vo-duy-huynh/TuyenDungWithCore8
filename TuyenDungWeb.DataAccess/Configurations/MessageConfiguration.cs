using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageChat>
    {
        public void Configure(EntityTypeBuilder<MessageChat> builder)
        {
            builder.ToTable("MessageChats");

            builder.Property(s => s.Content).IsRequired().HasMaxLength(500);

            builder.HasOne(s => s.ToRoom)
                .WithMany(m => m.MessageChats)
                .HasForeignKey(s => s.ToRoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
