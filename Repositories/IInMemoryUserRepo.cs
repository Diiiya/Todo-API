using System;
using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IInMemoryUserRepo
    {
        List<User> GetAllUsers();
        User GetUser(Guid id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(Guid id);
    }
}
