using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBuyer> ProductBuyers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                    .HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductCategory>()
                    .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductCategories)
                    .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<ProductCategory>()
                    .HasOne(p => p.Category)
                    .WithMany(pc => pc.ProductCategories)
                    .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<ProductBuyer>()
                    .HasKey(pb => new { pb.ProductId, pb.BuyerId });
            modelBuilder.Entity<ProductBuyer>()
                    .HasOne(p => p.Product)
                    .WithMany(pb => pb.ProductBuyers)
                    .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<ProductBuyer>()
                    .HasOne(p => p.Buyer)
                    .WithMany(pb => pb.ProductBuyers)
                    .HasForeignKey(b => b.BuyerId);
        }
    }
}
