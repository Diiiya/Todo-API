using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Api.DTOs;
using Todo.Api.Models;

namespace Todo.Api.Interfaces
{
    public interface IToDoRepo : IRepository<ToDo>
    {
        Task<List<ToDo>> GetAllTodosByUser(Guid userId);
    }
}