using System;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
               modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(11);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.UserPermissions).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.ToTable(tb => tb.HasCheckConstraint("CK_User_Permissions", "[UserPermissions] IN (1, 2)"));

                entity.Property(e => e.RefreshToken).IsRequired(false);
                entity.Property(e => e.RefreshTokenExpiryTime).IsRequired(false);

            });

               modelBuilder.Entity<Address>(entity =>
           {
                entity.HasKey(e => e.AddressId);
                entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AddressType).HasConversion<string>().HasMaxLength(50); // تخزين النوع كنص
                entity.Property(e => e.AddressDetails).IsRequired(false);

               // تعريف علاقة واحد إلى متعدد
               entity.HasOne(a => a.User).WithMany(u => u.Addresses).HasForeignKey(a => a.UserId);
            });

        }

        public DbSet<User> Users { get; set; }
    }
}
