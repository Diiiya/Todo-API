using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;

namespace TodoApi.Data
{
    // This adds some static data to the database 
    public class TestData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.EnsureCreated();

                if (context.User != null && context.User.Any())
                    return;

                var users = TestData.GetAllUsers().ToArray();
                context.User.AddRange(users);
                context.SaveChanges();
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> userList = new()
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "User1",
                    Email = "user1@mail.com",
                    Password = "Cvb123",
                    CreatedDate = DateTimeOffset.UtcNow,
                    Deleted = false
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "User2",
                    Email = "user2@mail.com",
                    Password = "op[098",
                    CreatedDate = DateTimeOffset.UtcNow,
                    Deleted = false
                }
            };
            return userList;
        }
    }
}