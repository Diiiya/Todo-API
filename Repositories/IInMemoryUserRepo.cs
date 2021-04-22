using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IInMemoryUserRepo
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(Guid id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(Guid id);
    }
}
