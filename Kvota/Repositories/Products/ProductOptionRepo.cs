using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Kvota.Repositories.Products
{
    public class ProductOptionRepo:BaseRepo<ProductOption>
    {
        public ProductOptionRepo(KvotaProductContext context) : base(context)
        {
            Table = context.ProductOptions;
        }
    }
}
