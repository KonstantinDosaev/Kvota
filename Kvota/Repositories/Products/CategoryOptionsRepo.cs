using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Kvota.Repositories.Products
{
    public class CategoryOptionsRepo:BaseRepo<CategoryOption>
    {
        public CategoryOptionsRepo(KvotaProductContext context) : base(context)
        {
            Table = context.CategoryOptions;
        }
    }
}
