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
                .Property(p => p.Email).HasMaxLength(50)
                .IsRequired();
            builder.Entity<User>()
                .Property(p => p.Password).HasMaxLength(40)
                .IsRequired();

            builder.Entity<User>().ToTable("User");
        }

        public DbSet<User> User { get; set; }
    }
}