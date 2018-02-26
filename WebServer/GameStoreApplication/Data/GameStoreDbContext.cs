namespace WebServer.GameStoreApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GameStoreDbContext : DbContext
    {
        public GameStoreDbContext() { }

        public GameStoreDbContext(DbContextOptions options)
            :base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.GameId, ug.UserId});

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(ug => ug.User);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Users)
                .WithOne(ug => ug.Game);
        }
    }
}
