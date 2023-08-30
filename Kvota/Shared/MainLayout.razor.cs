using Kvota.Interfaces;
using Kvota.Migrations;
using Kvota.Models.Content;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Kvota.Constants;

namespace Kvota.Shared
{
    partial class MainLayout
    {
        private ContactsModel Contact { get; set; } = default!;
        private string buttonDisplay = "none";
        
        protected override async Task OnInitializedAsync()
        {
            //using var scope = serviceScopeFactory.CreateScope();
            //Contact = await scope.ServiceProvider.GetService<IRepo<ContactsModel>>()!.GetOneAsync(new Guid("80beea30-3f74-42f3-812b-561cea25ec32"));

            await using var openStream = File.OpenRead(Links.ContactsJson);
            Contact = (await JsonSerializer.DeserializeAsync<ContactsModel>(openStream))!;
            //await using (var fs = new FileStream("contactsContent.json", FileMode.OpenOrCreate))
            //{
            //    var contactsContent = Contact;
            //    await JsonSerializer.SerializeAsync(fs, contactsContent);
            //}
        }
    }
}
