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
            /*
             In cases where the data is read-only i.e. it is being used for display purposes on a web page and will not be modified during the current request, 
             it is not necessary to have the context perform the extra work required to set up tracking. The AsNoTracking method stops this work being done and can improve performance of an application
             a series of read-only queries to perform against the same instance of the context, you can configure the tracking behaviour at context-level instead of using the AsNoTracking method in each query
            */
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var allTasks = await context.Set<ToDo>().Where(todo => todo.FkUserId == userId).ToListAsync();
            var allTags = await context.Set<Tag>().Where(tag => tag.FkUserId == userId).ToListAsync();
            foreach (var item in allTasks)
            {
                Tag myTag = new();
                if (item.FkTagId != null)
                {
                    myTag = allTags.Find(tag => tag.Id == item.FkTagId);
                }
                item.Tag = myTag;
            }
            return allTasks;
        }
        public async Task<ToDo> Add(ToDo entity)
        {
            context.Set<ToDo>().Add(entity);
            if (entity.FkTagId != null)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                Tag myTag = await context.Set<Tag>().FindAsync(entity.FkTagId);
                entity.Tag = myTag;
            }
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