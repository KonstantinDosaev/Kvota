using BlazorBootstrap;
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
            Tools = await ContactServ.GetOneAsync(new Guid("80beea30-3f74-42f3-812b-561cea25ec32"));

        }

        private async void SubmitPlayer()
        {
            await ContactServ.Update(Tools);

            modal?.ShowAsync();
        }
    }
}
