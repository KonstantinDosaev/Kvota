using Kvota.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Kvota.Models.Products;

namespace Kvota.Repositories
{
    public abstract class BaseRepo<T> : IRepo<T> where T : class, IIdentifiable, new()
    {
        public DbContext Context { get; init; }

        protected BaseRepo(DbContext context)
        {
            Context = context;
        }


        protected DbSet<T> Table = null!;

        public virtual async Task<T> AddAsync(T entity)
        {
            await Table.AddAsync(entity);
            await SaveChangesAsync();

            return entity;
        }

        public async Task ManualSaveAsync()
        {
            await Context.SaveChangesAsync();
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

        public virtual async Task<bool> Update(T entity)
        {
            Table.Update(entity);
            var t= await SaveChangesAsync();
            return t>0;
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
        public IQueryable<T> GetAllByQuery() => Table;
        public virtual async Task<IEnumerable<T>> GetAllByIdAsync(Guid id, string name) => await Table.Where(w=>w.Id==id).ToListAsync();
     

        public virtual async Task<T> GetOneAsync(Guid id)
        {
            
                return (await Task.Run(() => Table.FirstOrDefault(entity => entity.Id == id)))!;
            

        }
        public T GetOne(Guid id)
        {

            return Table.FirstOrDefault(entity => entity.Id == id)!;


        }

        public virtual async Task<IEnumerable<T>> GetSearch(string searchString) => await Table.ToListAsync();


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
        internal virtual int SaveChanges()
        {
            try
            {
                return  Context.SaveChanges();
            }

            catch (Exception ex)
            {
                var err = ex as ILogger<ErrorContext>;
                throw;
            }
        }
        internal virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                ILogger<ErrorContext>? err = ex as ILogger<ErrorContext>;
                throw;
            }
        }

    }
}
