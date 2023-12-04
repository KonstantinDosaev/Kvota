using Kvota.Migrations;
using Kvota.Models.Products;

namespace Kvota.Repositories.Products
{
    public class ProductInStorageRepo : BaseRepo<ProductsInStorage>
    {
        public ProductInStorageRepo(KvotaProductContext context) : base(context)
        {
            Table = context.ProductsInStorages;
        }
    }
}
