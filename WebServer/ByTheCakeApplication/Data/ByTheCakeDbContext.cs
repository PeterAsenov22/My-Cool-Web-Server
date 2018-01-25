namespace WebServer.ByTheCakeApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ByTheCakeDbContext : DbContext
    {
        public ByTheCakeDbContext() { }

        public ByTheCakeDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new {op.OrderId, op.ProductId});

            modelBuilder.Entity<Product>()
                .HasMany(pr => pr.Orders)
                .WithOne(op => op.Product);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithOne(op => op.Order);
        }
    }
}

