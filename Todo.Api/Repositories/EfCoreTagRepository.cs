using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;
using Todo.Api.Interfaces;
using System.Linq;

namespace Todo.Api.Data.EfCore
{
    public class EfCoreTagRepository : ITagRepo
    {
        private readonly DataContext context;
        public EfCoreTagRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<List<Tag>> GetAll()
        {
            return await context.Set<Tag>().ToListAsync();
        }
        public async Task<Tag> Get(Guid id)
        {
            return await context.Set<Tag>().FindAsync(id);
        }
        public async Task<List<Tag>> GetAllTagsByUser(Guid userId)
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var allTags = await context.Set<Tag>().Where(tag => tag.FkUserId == userId).ToListAsync();

            return allTags;
        }
        public async Task<Tag> Add(Tag entity)
        {
            context.Set<Tag>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> Update(Tag entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> Delete(Guid id)
        {
            var entity = await context.Set<Tag>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<Tag>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}