using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;
using Todo.Api.Interfaces;

namespace Todo.Api.Data.EfCore
{
    public class EfCoreToDoRepository : IToDoRepo
    {
        private readonly DataContext context;
        public EfCoreToDoRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<ToDo>> GetAll()
        {
            return await context.Set<ToDo>().ToListAsync();
        }
        public async Task<ToDo> Get(Guid id)
        {
            return await context.Set<ToDo>().FindAsync(id);
        }
        public async Task<List<ToDo>> GetAllTodosByUser(Guid userId)
        {
            var allTasks = await context.Set<ToDo>().Where(todoo => todoo.FkUserId == userId).ToListAsync();
            return allTasks;
        }
        public async Task<ToDo> Add(ToDo entity)
        {
            context.Set<ToDo>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<ToDo> Update(ToDo entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<ToDo> Delete(Guid id)
        {
            var entity = await context.Set<ToDo>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<ToDo>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}