using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Kvota.Repositories.Products
{
    public class GrandCategoryRepo: BaseRepo<GrandCategory>
    {
        public GrandCategoryRepo(KvotaProductContext context) : base(context)
        {
            Table = context.GrandCategories;
        }
        public override async Task<IEnumerable<GrandCategory>> GetAllAsync() => await Table.Include(i => i.Categories).ToListAsync();
    }
}
