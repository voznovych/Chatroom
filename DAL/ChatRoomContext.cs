namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class ChatRoomContext : DbContext
    {        
        public ChatRoomContext()
            : base("name=ChatRoom")
        {
            Database.SetInitializer<ChatRoomContext>(new ChatRoomInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User config
            modelBuilder.Configurations.Add(new UserConfig());

            // Room config
            modelBuilder.Configurations.Add(new RoomConfig());

            // Sex Config
            modelBuilder.Entity<Sex>()
                .Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();

            //Country Config
            modelBuilder.Entity<Country>()
                .Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired();

            // Genres
            modelBuilder.Entity<Genre>()
                .Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            // User Statuses
            modelBuilder.Entity<UserStatus>()
                .Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            // RoomVisitInfos
            modelBuilder.Entity<VisitInfo>()
                .HasRequired(v => v.User)
                .WithMany(u => u.VisitInfos)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VisitInfo>()
                .HasRequired(v => v.Room)
                .WithMany(r => r.VisitInfos)
                .HasForeignKey(a => a.RoomId)
                .WillCascadeOnDelete(false);

            // Messages
            modelBuilder.Entity<Message>()
                .Property(c => c.Text)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);    

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Room)
                .WithMany(r => r.Messages)
                .HasForeignKey(a => a.RoomId)
                .WillCascadeOnDelete(false);            
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Room> Rooms { get; set; } 
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<VisitInfo> VisitInfos { get; set; }
        public virtual DbSet<UserStatus> UserStatuses { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
    }
     
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public string Text { get; set; }
        public DateTime DateOfSend { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public int SexId { get; set; }
        public virtual Sex Sex { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public byte[] Photo { get; set; }
        public int StatusId { get; set; }
        public virtual UserStatus Status { get; set; }

        // collection of sended messages
        public virtual ICollection<Message> Messages { get; set; }
        // collection of invited rooms
        public virtual ICollection<Room> Rooms { get; set; }
        // collection of created rooms
        public virtual ICollection<Room> CreatedRooms { get; set; }
        // collection of user visit infos
        public virtual ICollection<VisitInfo> VisitInfos { get; set; }
    } 

    public class Room
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public string Name { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPersonal { get; set; }

        // collection of receivered messages
        public virtual ICollection<Message> Messages { get; set; }
        // collection of invited users
        public virtual ICollection<User> Users { get; set; }
        // collection of room visit infos
        public virtual ICollection<VisitInfo> VisitInfos{ get; set; }
    }

    public class VisitInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public DateTime LastDateOfVisit { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // collection of rooms
        public virtual ICollection<Room> Rooms { get; set; }
    }

    public class Sex
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // collection of users
        public virtual ICollection<User> Users { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // collection of users
        public virtual ICollection<User> Users { get; set; }
    }

    public class UserStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // collection of users
        public virtual ICollection<User> Users { get; set; }
    }
}