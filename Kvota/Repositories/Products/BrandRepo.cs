using Kvota.Migrations;
using Kvota.Models.Products;


namespace Kvota.Repositories.Products
{
    public class BrandRepo:BaseRepo<Brand>
    {
        public BrandRepo(KvotaProductContext context) : base(context)
        {
            Table = context.Brands;
        }
    }
}
