using System.Text.Json;
using Kvota.Constants;
using Kvota.Models.Content;

namespace Kvota.Pages
{
    partial class Contacts
    {
        private ContactsModel Contact { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await using var openStream = File.OpenRead($"{Links.RootPath}/{Links.ContactsJson}");
            Contact = (await JsonSerializer.DeserializeAsync<ContactsModel>(openStream))!;
        }
    }
}
