using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;
using Todo.Api.Interfaces;

namespace Todo.Api.Data.EfCore
{
    public class EfCoreUserRepository : IUserRepo
    {
        private readonly DataContext context;
        public EfCoreUserRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<List<User>> GetAll()
        {
            return await context.Set<User>().ToListAsync();
        }

        public async Task<User> Get(Guid id)
        {
            return await context.Set<User>().FindAsync(id);
        }
        public async Task<User> Add(User entity)
        {
            context.Set<User>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Update(User entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Delete(Guid id)
        {
            var entity = await context.Set<User>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<User>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

    }
}