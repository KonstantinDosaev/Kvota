using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Kvota.Repositories.Products
{
    public class ProductInStorageRepo
    {
        public ProductInStorageRepo(KvotaProductContext context)
        {
            Context = context;
        }

        public DbContext Context { get; init; }

        protected DbSet<ProductsInStorage> Table;

        public async Task<ProductsInStorage> AddAsync(ProductsInStorage entity)
        {
            await Table.AddAsync(entity);
            await SaveChangesAsync();

            return entity;
        }

        public async Task ManualSaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public ProductsInStorage Add(ProductsInStorage entity)
        {
            Table.Add(entity);
            SaveChanges();

            return entity;
        }
        public async Task<IEnumerable<ProductsInStorage>> AddRangeAsync(IEnumerable<ProductsInStorage> entities)
        {
            await Table.AddRangeAsync(entities);
            await SaveChangesAsync();

            return entities;
        }

        public virtual async Task<bool> Update(ProductsInStorage entity)
        {
            Table.Update(entity);
            var t =
            await SaveChangesAsync();
            return t > 0;
        }

        //public async Task<int> DeleteAsync(Guid id)
        //{
        //    var entity = await GetOneAsync(id);

        //    if (entity == null) return 0;

        //    Context.Entry(entity).State = EntityState.Deleted;
        //    return await SaveChangesAsync();

        //}

        //public async Task<int> DeleteRangeAsync(IEnumerable<Guid> ids)
        //{
        //    var result = 0;

        //    foreach (var id in ids)
        //    {
        //        var affectedValue = await DeleteAsync(id);
        //        result += affectedValue;
        //    }

        //    return result;
        //}

        //public virtual async Task<IEnumerable<ProductsInStorage>> GetAllAsync() => await Table.ToListAsync();
        //public virtual async Task<IEnumerable<ProductsInStorage>> GetAllByIdAsync(Guid id, string name) => await Table.Where(w => w.Id == id).ToListAsync();


        //public virtual async Task<ProductsInStorage> GetOneAsync(Guid id)
        //{

        //    return (await Task.Run(() => Table.FirstOrDefault(entity => entity.Id == id)))!;


        //}
        //public ProductsInStorage GetOne(Guid id)
        //{

        //    return Table.FirstOrDefault(entity => entity.Id == id)!;


        //}

        public virtual async Task<IEnumerable<ProductsInStorage>> GetSearch(string searchString) => await Table.ToListAsync();


        public async Task<int> SaveAsync(ProductsInStorage entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }
        public int Save(ProductsInStorage entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }
        internal virtual int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
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
