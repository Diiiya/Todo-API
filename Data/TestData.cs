using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;
using ToDoAPI.Extensions;

namespace TodoApi.Data
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
                context.SaveChanges();
                context.Tag.AddRange(tags);
                context.SaveChanges();
                context.ToDo.AddRange(todos);
                context.SaveChanges(); // can i save only once or for each i should?
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
                    Password = passwordHasher.hashPass("Cvb123"),
                    CreatedDate = DateTimeOffset.UtcNow,
                    Deleted = false
                },
                new User
                {
                    Id = Guid.NewGuid(),
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
                    Date = DateTimeOffset.UtcNow.Date,
                    Time = DateTimeOffset.UtcNow,
                    Location = "Copenhagen",
                    Done = false,
                    Priority = 2,
                    FkTagId = Guid.NewGuid(), //here should be tag id 
                    FkUserId = Guid.NewGuid() //here should be user id 
                },
                new ToDo
                {
                    Id = Guid.NewGuid(),
                    Description = "Clean the room",
                    Date = DateTimeOffset.UtcNow.Date,
                    Time = DateTimeOffset.UtcNow,
                    Location = "Roskilde",
                    Done = false,
                    Priority = 1,
                    FkTagId = Guid.NewGuid(), //here should be tag id 
                    FkUserId = Guid.NewGuid() //here should be user id 
                }
            };
            return todoList;
        }

        public static List<Tag> GetAllTags()
        {
            List<Tag> tagList = new()
            {
                new Tag
                {
                    Id = Guid.NewGuid(),
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