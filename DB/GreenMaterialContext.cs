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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Product>().ToTable("Product");
        }

    }
}