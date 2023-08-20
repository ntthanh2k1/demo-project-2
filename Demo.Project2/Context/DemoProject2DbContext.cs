using Demo.Project2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Demo.Project2.Context
{
    public class DemoProject2DbContext : DbContext
    {
        public DemoProject2DbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Category>? Categories { get; set; }
        public virtual DbSet<Review>? Reviews { get; set; }
        //public virtual DbSet<Image>? Images { get; set; }
        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<OrderDetails>? OrderDetails { get; set; }
        public virtual DbSet<Product>? Products { get; set; }
        public virtual DbSet<Role>? Roles { get; set; }
        public virtual DbSet<Slide>? Slides { get; set; }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<UserRole>? UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).HasMaxLength(250);
                entity.Property(a => a.Name).HasMaxLength(250);
                entity.HasOne(a => a.ParentCategory)
                    .WithMany(b => b.ChildCategories)
                    .HasForeignKey(c => c.ParentId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");
                entity.Property(a => a.Username).HasMaxLength(250);
                entity.Property(a => a.Sentiment).HasMaxLength(50);
                entity.HasKey(a => a.Id);
                entity.HasOne(a => a.User)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_User");
                entity.HasOne(a => a.Product)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(c => c.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Product");
            });

            //modelBuilder.Entity<Image>(entity =>
            //{
            //    entity.ToTable("Image");
            //    entity.HasKey(a => a.Id);
            //    entity.Property(a => a.Code).HasMaxLength(250);
            //    entity.Property(a => a.Name).HasMaxLength(250);
            //    entity.Property(a => a.ImageName).HasMaxLength(250);
            //    entity.HasOne(a => a.Product)
            //        .WithMany(b => b.Images)
            //        .HasForeignKey(c => c.ProductId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Image_Product");
            //});

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).HasMaxLength(250);
                entity.Property(a => a.Name).HasMaxLength(250);
                entity.Property(a => a.FullName).HasMaxLength(250);
                entity.Property(a => a.PhoneNumber).HasMaxLength(50);
                entity.HasOne(a => a.User)
                    .WithMany(b => b.Orders)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_User");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(a => new { a.OrderId, a.ProductId });
                entity.Property(a => a.Name).HasMaxLength(250);
                entity.HasOne(a => a.Order)
                    .WithMany(b => b.OrderDetails)
                    .HasForeignKey(c => c.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Order");
                entity.HasOne(a => a.Product)
                    .WithMany(b => b.OrderDetails)
                    .HasForeignKey(c => c.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).HasMaxLength(250);
                entity.Property(a => a.Name).HasMaxLength(250);
                entity.Property(a => a.Image).HasMaxLength(250);
                entity.HasOne(a => a.Category)
                    .WithMany(b => b.Products)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).HasMaxLength(250);
                entity.Property(a => a.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<Slide>(entity =>
            {
                entity.ToTable("Slide");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).HasMaxLength(250);
                entity.Property(a => a.Name).HasMaxLength(250);
                entity.Property(a => a.Image).HasMaxLength(250);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Username).HasMaxLength(250);
                entity.Property(a => a.Password).HasMaxLength(250);
                entity.Property(a => a.FullName).HasMaxLength(250);
                entity.Property(a => a.Email).HasMaxLength(250);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");
                entity.HasKey(a => new { a.UserId, a.RoleId });
                entity.HasOne(a => a.User)
                    .WithMany(b => b.UserRoles)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
                entity.HasOne(a => a.Role)
                    .WithMany(b => b.UserRoles)
                    .HasForeignKey(c => c.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");
            });
        }
    }
}