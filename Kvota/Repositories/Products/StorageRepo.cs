using Kvota.Migrations;
using Kvota.Models.Products;

namespace Kvota.Repositories.Products
{
    public class StorageRepo: BaseRepo<Storage>
    {
        public StorageRepo(KvotaProductContext context) : base(context)
        {
            Table = context.Storages;
        }
    }
}
