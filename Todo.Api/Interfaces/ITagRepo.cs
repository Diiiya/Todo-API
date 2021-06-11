using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Api.Models;

namespace Todo.Api.Interfaces
{
    public interface ITagRepo : IRepository<Tag>
    {
        Task<List<Tag>> GetAllTagsByUser(Guid userId);
    }
}