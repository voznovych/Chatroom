using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    internal class RoomConfig : EntityTypeConfiguration<Room>
    {
        public RoomConfig()
        {            
            HasRequired(r => r.Creator)
                .WithMany(u => u.CreatedRooms)
                .HasForeignKey(a => a.CreatorId)
                .WillCascadeOnDelete(false);
            
            Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();
            
            HasRequired(r => r.Genre)
                .WithMany(u => u.Rooms)
                .HasForeignKey(a => a.GenreId)
                .WillCascadeOnDelete(false);

            // many users with many rooms            
            HasMany(r => r.Users)
                .WithMany(u => u.Rooms);
        }
    }
}