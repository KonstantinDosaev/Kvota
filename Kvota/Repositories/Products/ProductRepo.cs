using Kvota.Data;
using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Kvota.Repositories.Products
{
    public class ProductRepo:BaseRepo<Product>
    {
        public ProductRepo(KvotaProductContext context) : base(context)
        {
            Table = context.Products;
            Context = context;
        
        }
        public override async Task<IEnumerable<Product>> GetAllAsync() => await Table.OrderBy(o => o.Name)
            .Include(i => i.Brand).Include(i => i.Category)
            .Include(i => i.ProductOption).ToListAsync();

        public override async Task<IEnumerable<Product>> GetSearch(string searchString)
        {
            return await Table
                .Where(x =>   x.Name.Contains(searchString)||( x.PartNumber != null &&  x.PartNumber.Contains(searchString))).ToListAsync();
        }
        public override async Task<IEnumerable<Product>> GetAllByIdAsync(Guid id) => await Table.OrderBy(o => o.Name).Where(w => w.CategoryId == id)
            .Include(i => i.Brand).Include(i => i.Category)
            .Include(i => i.ProductOption).ToListAsync();

    }
}
