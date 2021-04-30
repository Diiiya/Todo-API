using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<User>()
                .Property(p => p.Id)
                .IsRequired();
            builder.Entity<User>()
                .Property(p => p.Username).HasMaxLength(30)
                .IsRequired();
            builder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired();
            builder.Entity<User>()
                .Property(p => p.Password).HasMaxLength(100)
                .IsRequired();

            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>().ToTable("User");
        }

        public DbSet<User> User { get; set; }
    }
}