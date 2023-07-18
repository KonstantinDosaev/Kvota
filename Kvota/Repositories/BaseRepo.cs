using Kvota.Data;
using Kvota.Interfaces;
using Kvota.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Kvota.Repositories
{
    public abstract class BaseRepo<T> : IRepo<T> where T : class, IIdentifiable, new()
    {
        public KvotaContext Context { get; init; }
        public BaseRepo(KvotaContext context)
        {
            Context = context;
        }


        protected DbSet<T> Table;

        public async Task<T> AddAsync(T entity)
        {
            await Table.AddAsync(entity);
            await SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
            await SaveChangesAsync();

            return entities;
        }
        public async Task Update(T entity)
        {
            Table.Update(entity);
            await SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var entity = await GetOneAsync(id);

            if (entity == null) return 0;

            Context.Entry(entity).State = EntityState.Deleted;
            return await SaveChangesAsync();

        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var result = 0;

            foreach (var id in ids)
            {
                var affectedValue = await DeleteAsync(id);
                result += affectedValue;
            }

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await Table.ToListAsync();
        private object locker = new();
        public virtual async Task<T> GetOneAsync(Guid id)
        {
            try
            {
                return (await Task.Run(() => Table.FirstOrDefault(entity => entity.Id == id)))!;
            }
            catch
            {
                return Table.FirstOrDefault(e => e.Id == id)!;
            }
            
            

        }
 

        public async Task<int> SaveAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
