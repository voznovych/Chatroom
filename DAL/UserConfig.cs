using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    internal class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            Property(u => u.Login)
                .HasMaxLength(50)
                .IsRequired();

            Property(u => u.Password)
                .IsRequired();

            Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();
            
            Property(u => u.Surname)
                .HasMaxLength(50)
                .IsRequired();

            // required sex with many users
            HasRequired(u => u.Sex)
                .WithMany(s => s.Users)
                .HasForeignKey(a => a.SexId)
                .WillCascadeOnDelete(false);

            // optional country with many users
            HasOptional(u => u.Country)
                .WithMany(c => c.Users)
                .HasForeignKey(a => a.CountryId);

            // required status with many users
            HasRequired(u => u.Status)
                .WithMany(s => s.Users)
                .HasForeignKey(a => a.StatusId)
                .WillCascadeOnDelete(false);
        }
    }
}