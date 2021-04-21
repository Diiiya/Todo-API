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
            new User(Guid.NewGuid(), "Username", "Email", "", DateTimeOffset.UtcNow, false),
            new User(Guid.NewGuid(), "Username2", "Email", "", DateTimeOffset.UtcNow, false)
        };

        public List<User> GetAllUsers()
        {
            return userList;
        }

        public User GetUser(Guid id)
        {
            return userList.Where(user => user.Id == id).SingleOrDefault();
        }
    }

}