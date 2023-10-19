using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DB
{
    public class GreenMaterialContext : DbContext
    {
        public GreenMaterialContext(DbContextOptions<GreenMaterialContext> options) 
            : base (options)
        {

        }

        public DbSet<User> users { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<Item> items { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Delivery> deliveries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Delivery>().ToTable("Delivery");

            modelBuilder.Entity<Item>()
                 .Property(i => i.cantidad)
                 .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.price)
                .HasColumnType("decimal(18, 2)");
        }

    }
}