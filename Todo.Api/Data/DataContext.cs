using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;

namespace Todo.Api.Data
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
            
            // to set primary key
            builder.Entity<User>()
            .HasKey(p => p.Id);

            builder.Entity<User>().ToTable("User");

            

            builder.Entity<ToDo>()
            .Property(p => p.Id)
            .IsRequired();
            builder.Entity<ToDo>()
            .Property(p => p.Description).HasMaxLength(255)
            .IsRequired();
            builder.Entity<ToDo>()
            .Property(p => p.Date);
            builder.Entity<ToDo>()
            .Property(p => p.Time);
            builder.Entity<ToDo>()
            .Property(p => p.Location).HasMaxLength(255);
            builder.Entity<ToDo>()
            .Property(p => p.Done)
            .IsRequired();
            builder.Entity<ToDo>()
            .Property(p => p.Priority)
            .IsRequired();
            builder.Entity<ToDo>()
            .Property(p => p.FkTagId)
            .IsRequired();
            builder.Entity<ToDo>()
            .Property(p => p.FkUserId)
            .IsRequired();

            // to set primary key
            builder.Entity<ToDo>()
            .HasKey(p => p.Id);

            // to set foreign keys
            builder.Entity<ToDo>()
            .HasOne(p => p.Tag)
            .WithMany(t => t.ToDos)
            .HasForeignKey(p => p.FkTagId);
            builder.Entity<ToDo>()
            .HasOne(p => p.User)
            .WithMany(u => u.ToDos)
            .HasForeignKey(p => p.FkUserId);
            
            
            builder.Entity<ToDo>().ToTable("ToDo");
            
            builder.Entity<Tag>()
            .Property(p => p.Id)
            .IsRequired();
            builder.Entity<Tag>()
            .Property(p => p.TagName).HasMaxLength(20)
            .IsRequired();
            builder.Entity<Tag>()
            .Property(p => p.TagColor).HasMaxLength(10)
            .IsRequired();

            // to set primary key
            builder.Entity<Tag>()
            .HasKey(p => p.Id);
            
            builder.Entity<Tag>().ToTable("Tag");
        }

        public DbSet<User> User { get; set; }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<Tag> Tag { get; set; }
    }
}