using Kvota.Migrations;
using Kvota.Models.Products;

namespace Kvota.Repositories.Products
{
    public class CategoryRepo : BaseRepo<Category>
    {
        public CategoryRepo(KvotaProductContext context) : base(context)
            {
                Table = context.Categories;
            }
        
    }
}
