using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class InMemoryUserRepo : IInMemoryUserRepo
    {
        private readonly List<User> userList = new()
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "Username",
                Email = "Email",
                Password = "",
                CreatedDate = DateTimeOffset.UtcNow,
                Deleted = false
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "Username2",
                Email = "Email",
                Password = "",
                CreatedDate = DateTimeOffset.UtcNow,
                Deleted = false
            }
        };

        public void CreateUser(User user)
        {
            userList.Add(user);
        }

        public void DeleteUser(Guid id)
        {
            var index = userList.FindIndex(user => user.Id == id);
            userList.RemoveAt(index);
        }

        public List<User> GetAllUsers()
        {
            return userList;
        }

        public User GetUser(Guid id)
        {
            return userList.Where(user => user.Id == id).SingleOrDefault();
        }

        public void UpdateUser(User user)
        {
            var index = userList.FindIndex(existingUser => existingUser.Id == user.Id);
            userList[index] = user;
        }
    }

}