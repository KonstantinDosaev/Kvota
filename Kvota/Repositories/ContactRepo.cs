using Kvota.Migrations;
using Kvota.Models.Content;

namespace Kvota.Repositories
{
    public class ContactRepo: BaseRepo<ContactsModel>
    {
        public ContactRepo(KvotaContext context) : base(context)
        {
            //Table = context.Contacts;
        }
    }
}
