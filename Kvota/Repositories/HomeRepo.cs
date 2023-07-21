using Kvota.Data;
using Kvota.Models.Content;

namespace Kvota.Repositories
{
    public class HomeRepo: BaseRepo<Home>
    {
        public HomeRepo(KvotaContext context) : base(context)
        {
            Table = context.Homes;
        }
    }
}
