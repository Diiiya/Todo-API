using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Todo.Api.Models;
using Todo.Api.Extensions;

namespace Todo.Api.Data
{
    // This adds some static data to the database 
    public class TestData
    {
        static PasswordHasher passwordHasher = new PasswordHasher();
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.EnsureCreated();

                if (context.User != null && context.User.Any())
                    return;

                var users = TestData.GetAllUsers().ToArray();
                var tags = TestData.GetAllTags().ToArray();
                var todos = TestData.GetAllToDos().ToArray();
                context.User.AddRange(users);
                context.Tag.AddRange(tags);
                context.ToDo.AddRange(todos);
                context.SaveChanges(); 
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> userList = new()
            {
                new User
                {
                    Id = Guid.Parse("ae2d605e-2392-4a86-b3a2-bf75c486f311"),
                    Username = "User1",
                    Email = "user1@mail.com",
                    Password = passwordHasher.hashPass("Cvb123"),
                    CreatedDate = DateTimeOffset.UtcNow,
                    Deleted = false
                },
                new User
                {
                    Id = Guid.Parse("ae2d605e-2392-4a86-b3a2-bf75c486f322"),
                    Username = "User2",
                    Email = "user2@mail.com",
                    Password = passwordHasher.hashPass("op[098"),
                    CreatedDate = DateTimeOffset.UtcNow,
                    Deleted = false
                }
            };
            return userList;
        }

        public static List<ToDo> GetAllToDos()
        {
            List<ToDo> todoList = new()
            {
                new ToDo
                {
                    Id = Guid.NewGuid(),
                    Description = "Do laundry",
                    DateTime = DateTimeOffset.UtcNow,
                    Location = "Copenhagen",
                    Done = false,
                    Priority = 2,
                    FkTagId = Guid.Parse("ae2d605e-2392-4a86-b3a2-bf75c486f332"),
                    FkUserId = Guid.Parse("ae2d605e-2392-4a86-b3a2-bf75c486f311")
                },
            };
            return todoList;
        }

        public static List<Tag> GetAllTags()
        {
            List<Tag> tagList = new()
            {
                new Tag
                {
                    Id = Guid.Parse("ae2d605e-2392-4a86-b3a2-bf75c486f332"),
                    TagName = "Room",
                    TagColor = "Yellow"
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    TagName = "Household",
                    TagColor = "Red"
                }
            };
            return tagList;
        }
    }
}