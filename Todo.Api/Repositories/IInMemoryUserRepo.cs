using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Api.Models;

namespace Todo.Api.Repositories
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
