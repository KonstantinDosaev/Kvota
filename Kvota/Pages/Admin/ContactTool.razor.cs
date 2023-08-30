using System.Text.Encodings.Web;
using System.Text.Json;
using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Interfaces;
using Kvota.Models.Content;
using Microsoft.AspNetCore.Components;

namespace Kvota.Pages.Admin
{
    partial class ContactTool
    {
        [Inject] protected IRepo<ContactsModel> ContactServ { get; set; } = default!;

        private ContactsModel Tools { get; set; } = default!;

        private Modal? modal;



        protected override async Task OnInitializedAsync()
        {
            await using var openStream = File.OpenRead(Links.ContactsJson);
            Tools = (await JsonSerializer.DeserializeAsync<ContactsModel>(openStream))!;
        }

        private async void SubmitPlayer()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            await using var createStream = File.Create(Links.ContactsJson);
            await JsonSerializer.SerializeAsync(createStream, Tools, options);
            await createStream.DisposeAsync();
            modal?.ShowAsync();

            modal?.ShowAsync();
        }
    }
}
