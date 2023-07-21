using Kvota.Data;
using Kvota.Migrations;
using Kvota.Models.Products;

namespace Kvota.Repositories.Products
{
    public class ProductRepo:BaseRepo<Product>
    {
        public ProductRepo(KvotaProductContext context) : base(context)
        {
            Table = context.Products;

        
        }

    }
}
