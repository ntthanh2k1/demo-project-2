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

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Username).HasMaxLength(250);

                entity.Property(a => a.Password).HasMaxLength(250);

                entity.Property(a => a.FullName).HasMaxLength(250);

                entity.Property(a => a.Email).HasMaxLength(250);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
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

            //modelBuilder.Entity<Category>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(250);

            //    entity.HasOne(d => d.Parent)
            //        .WithMany(p => p.InverseParents)
            //        .HasForeignKey(d => d.ParentId)
            //        .HasConstraintName("FK_Category_Category");
            //});

            //modelBuilder.Entity<Product>(entity =>
            //{
            //    entity.HasOne(d => d.Category)
            //        .WithMany(p => p.Products)
            //        .HasForeignKey(d => d.CategoryId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Category_Product");
            //});

            //modelBuilder.Entity<Photo>(entity =>
            //{
            //    entity.HasOne(d => d.Product)
            //        .WithMany(p => p.Photos)
            //        .HasForeignKey(d => d.ProductId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Product_Photo");
            //});

            //modelBuilder.Entity<Invoice>(entity =>
            //{
            //    entity.HasOne(d => d.Account)
            //        .WithMany(p => p.Invoices)
            //        .HasForeignKey(d => d.AccountId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Invoice_Account");
            //});

            //modelBuilder.Entity<InvoiceDetails>(entity =>
            //{
            //    entity.HasKey(e => new { e.InvoiceId, e.ProductId });

            //    entity.HasOne(d => d.Invoice)
            //        .WithMany(p => p.InvoiceDetailses)
            //        .HasForeignKey(d => d.InvoiceId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_InvoiceDetails_Invoice");

            //    entity.HasOne(d => d.Product)
            //        .WithMany(p => p.InvoiceDetailses)
            //        .HasForeignKey(d => d.ProductId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_InvoiceDetails_Product");
            //});
        }
    }
}
