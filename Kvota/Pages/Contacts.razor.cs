using Kvota.Constants;
using Kvota.Models.Content;

namespace Kvota.Pages
{
    partial class Contacts
    {
        private ContactsModel Contact { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Contact = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
        }
    }
}
