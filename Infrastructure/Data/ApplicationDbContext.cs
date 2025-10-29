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
                entity.Property(e => e.Phone).HasMaxLength(11).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.UserPermissions).IsRequired().HasColumnType("Nvarchar(50)");  // تخزين النوع كنص
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Phone).IsUnique();

                entity.Property(e => e.RefreshToken).IsRequired(false);
                entity.Property(e => e.RefreshTokenExpiryTime).IsRequired(false);

                entity.Property(e => e.PasswordResetOtp).IsRequired(false).HasMaxLength(6);
                entity.Property(e => e.OtpExpiryTime).IsRequired(false);
            });

               modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId);
                entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AddressType).HasConversion<string>().HasMaxLength(50); // تخزين النوع كنص
                entity.Property(e => e.AddressDetails).IsRequired(false);//Nvarchar(MAX)

                // تعريف علاقة واحد إلى متعدد
                entity.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

               modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IconName).IsRequired().HasMaxLength(50); // MAX Length
                entity.Property(e => e.ParentID).IsRequired(false);


                // العلاقة التالية هي علاقة ذاتية (Self-Referencing Relationship):
                // كل قسم (Category) ممكن يكون له قسم أب (Parent) واحد فقط، 
                // وفي نفس الوقت ممكن يحتوي على عدة أقسام فرعية (SubCategories).
                // استخدمنا DeleteBehavior.Restrict عشان نمنع حذف القسم الأب 
                // لو لسه عنده أقسام فرعية مرتبطة بيه.
                entity.HasOne(c => c.Parent)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentID)
                    .OnDelete(DeleteBehavior.Restrict); // لمنع حذف قسم رئيسي إذا كان لديه أقسام فرعية
            });

               modelBuilder.Entity<Product>(entity =>
            {
             entity.HasKey(e => e.ProductID);
             entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
             entity.Property(e => e.Barcode).IsRequired();  // 450 Length
             entity.Property(e => e.ProductDescription).IsRequired(false);  // MAX Length
             entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
             entity.Property(e => e.QuantityInStock).IsRequired(); // int
             entity.Property(e => e.MainImageURL).IsRequired(); // MAX Length
             entity.HasIndex(p => p.Barcode).IsUnique(); // Barcode is Unique

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
            });

               modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(o => o.OfferID);
                entity.Property(o => o.Title).IsRequired().HasMaxLength(200);
                entity.Property(o => o.DiscountPercentage).HasColumnType("decimal(5, 2)");
                entity.Property(o => o.Description).IsRequired(false); // MAX Length
                entity.Property(o => o.StartDate).IsRequired();
                entity.Property(o => o.EndDate).IsRequired();


                // -- إضافة جديدة: تعريف علاقة العرض بالمنتجات (واحد-إلى-متعدد) --
                entity.HasMany(o => o.Products)
                      .WithOne(p => p.Offer)
                      .HasForeignKey(p => p.OfferID)
                      .OnDelete(DeleteBehavior.SetNull); // عند حذف العرض، اجعل المنتجات المرتبطة به بدون عرض
            });


            // أضف هذا الكود إلى ApplicationDbContext الموجود

            // في OnModelCreating، أضف التالي:


            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderDate).IsRequired();
                entity.Property(o => o.DeliveryDate).IsRequired(false);
                entity.Property(o => o.DeliveryTimeSlot).HasMaxLength(50);
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);

                // العلاقات
                entity.HasOne(o => o.User)
                      .WithMany()
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Address)
                      .WithMany()
                      .HasForeignKey(o => o.AddressId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(o => o.OrderDetails)
                      .WithOne(od => od.Order)
                      .HasForeignKey(od => od.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Payments)
                      .WithOne(p => p.Order)
                      .HasForeignKey(p => p.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(od => od.OrderDetailId);
                entity.Property(od => od.Quantity).IsRequired();
                entity.Property(od => od.PriceAtOrder).HasColumnType("decimal(18,2)");
                entity.Property(od => od.Subtotal).HasColumnType("decimal(18,2)");

                entity.HasOne(od => od.Product)
                      .WithMany()
                      .HasForeignKey(od => od.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                entity.Property(p => p.PaymentDate).IsRequired();
                entity.Property(p => p.PaymentMethod).HasConversion<string>().HasMaxLength(50);
                entity.Property(p => p.PaymentStatus).HasConversion<string>().HasMaxLength(50);
                entity.Property(p => p.TransactionId).HasMaxLength(200);
            });

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
