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
    }
}
