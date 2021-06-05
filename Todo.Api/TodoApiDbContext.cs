using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Todo.Api
{
    public partial class TodoApiDbContext : DbContext
    {
        public TodoApiDbContext()
        {
        }

        public TodoApiDbContext(DbContextOptions<TodoApiDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<ToDo> ToDos { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.HasIndex(e => e.FkUserId, "IX_Tag_FkUserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TagColor)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.FkUserId);
            });

            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.ToTable("ToDo");

                entity.HasIndex(e => e.FkTagId, "IX_ToDo_FkTagId");

                entity.HasIndex(e => e.FkUserId, "IX_ToDo_FkUserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.HasOne(d => d.FkTag)
                    .WithMany(p => p.ToDos)
                    .HasForeignKey(d => d.FkTagId);

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.ToDos)
                    .HasForeignKey(d => d.FkUserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "IX_User_Email")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "IX_User_Username")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
