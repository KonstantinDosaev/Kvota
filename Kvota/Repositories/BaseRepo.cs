using Kvota.Data;
using Kvota.Interfaces;
using Kvota.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Microsoft.AspNetCore.Diagnostics;

namespace Kvota.Repositories
{
    public abstract class BaseRepo<T> : IRepo<T> where T : class, IIdentifiable, new()
    {
        public DbContext Context { get; init; }
        public BaseRepo(DbContext context)
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
        public  T Add(T entity)
        {
             Table.Add(entity);
             SaveChanges();

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
 
        public virtual async Task<T> GetOneAsync(Guid id)
        {
            
                return (await Task.Run(() => Table.FirstOrDefault(entity => entity.Id == id)))!;
            

        }
        public T GetOne(Guid id)
        {

            return Table.FirstOrDefault(entity => entity.Id == id)!;


        }


        public async Task<int> SaveAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }
        public int Save(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }
        internal int SaveChanges()
        {
            try
            {
                return  Context.SaveChanges();
            }

            catch (Exception ex)
            {
                ILogger<ErrorContext> err = ex as ILogger<ErrorContext>;
                throw;
            }
        }
        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                ILogger<ErrorContext> err = ex as ILogger<ErrorContext>;
                throw;
            }
        }
    }
}
