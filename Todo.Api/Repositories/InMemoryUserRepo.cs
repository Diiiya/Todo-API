using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Api.Models;
using System.Threading.Tasks;

namespace Todo.Api.Repositories
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

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.FromResult(userList);
        }

        public async Task<User> Get(Guid id)
        {
            User user = userList.Where(user => user.Id == id).SingleOrDefault();
            return await Task.FromResult(user);
        }

        public async Task Add(User user)
        {
            userList.Add(user);
            await Task.CompletedTask;
        }

        public async Task Update(User user)
        {
            var index = userList.FindIndex(existingUser => existingUser.Id == user.Id);
            userList[index] = user;
            await Task.CompletedTask;
        }

        public async Task Delete(Guid id)
        {
            var index = userList.FindIndex(user => user.Id == id);
            userList.RemoveAt(index);
            await Task.CompletedTask;
        }
    }

}