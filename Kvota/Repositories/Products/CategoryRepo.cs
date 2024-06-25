using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Kvota.Repositories.Products
{
    public class CategoryRepo : BaseRepo<Category>
    {
        public CategoryRepo(KvotaProductContext context) : base(context)
            {
                Table = context.Categories;
            }
        public override async Task<IEnumerable<Category>> GetAllAsync() => await Table.Include(i => i.CategoryOptions).
            Include(i=>i.Parent).Include(i=>i.Products).Include(i=>i.Children).ToListAsync();
        
    }
}
