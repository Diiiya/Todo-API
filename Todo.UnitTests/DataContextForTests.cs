using System;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Models;

namespace Todo.UnitTests
{
    public class DataContextForTests
    {
        protected DataContextForTests(DbContextOptions<DataContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        protected DbContextOptions<DataContext> ContextOptions { get; }

        private void Seed()
        {
            using (var context = new DataContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user1 = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "Username1",
                        Email = "email2@mail.com",
                        Password = "lala123",
                        CreatedDate = DateTimeOffset.UtcNow,
                        Deleted = false,
                        ToDos = null
                    };

                var user2 = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "Username2",
                        Email = "email@mail.com",
                        Password = "lala123",
                        CreatedDate = DateTimeOffset.UtcNow,
                        Deleted = false,
                        ToDos = null
                    };

                context.AddRange(user1, user2);
                context.SaveChanges();
            }
        }
    }
}
