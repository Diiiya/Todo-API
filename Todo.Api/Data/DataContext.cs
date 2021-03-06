using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;

namespace Todo.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // Fluent API used to define the database properties and configurations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // User table
            builder.Entity<User>().Property(p => p.Id).IsRequired();
            builder.Entity<User>().Property(p => p.Username).HasMaxLength(30).IsRequired();
            builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<User>().Property(p => p.Email).IsRequired();
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();  
            builder.Entity<User>().Property(p => p.Password).HasMaxLength(100).IsRequired();            
                      
            builder.Entity<User>().HasKey(p => p.Id);
            builder.Entity<User>().ToTable("User");


            // ToDo table
            builder.Entity<ToDo>().Property(p => p.Id).IsRequired();
            builder.Entity<ToDo>().Property(p => p.Description).HasMaxLength(255).IsRequired();
            builder.Entity<ToDo>().Property(p => p.DateTime);
            builder.Entity<ToDo>().Property(p => p.Location).HasMaxLength(255);
            builder.Entity<ToDo>().Property(p => p.Done).IsRequired();
            builder.Entity<ToDo>().Property(p => p.Priority);
            builder.Entity<ToDo>().Property(p => p.FkTagId);
            builder.Entity<ToDo>().Property(p => p.FkUserId).IsRequired();

            builder.Entity<ToDo>().HasKey(p => p.Id);
            builder.Entity<ToDo>().HasOne(p => p.Tag).WithMany(t => t.ToDos).HasForeignKey(p => p.FkTagId);
            builder.Entity<ToDo>().HasOne(p => p.User).WithMany(u => u.ToDos).HasForeignKey(p => p.FkUserId);          
            builder.Entity<ToDo>().ToTable("ToDo");


            // Tag table            
            builder.Entity<Tag>().Property(p => p.Id).IsRequired();
            builder.Entity<Tag>().Property(p => p.TagName).HasMaxLength(20).IsRequired();
            builder.Entity<Tag>().Property(p => p.TagColor).HasMaxLength(10).IsRequired();
            builder.Entity<Tag>().Property(p => p.FkUserId).IsRequired();

            builder.Entity<Tag>().HasKey(p => p.Id);     
            builder.Entity<Tag>().HasOne(p => p.User).WithMany(u => u.Tags).HasForeignKey(p => p.FkUserId);         
            builder.Entity<Tag>().ToTable("Tag");
        }

        public DbSet<User> User { get; set; }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<Tag> Tag { get; set; }
    }
}