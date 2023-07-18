using BlazorBootstrap;
using Kvota.Interfaces;
using Kvota.Models.Content;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components.Admin
{
    partial class ContactTool
    {
        [Inject]
        protected IRepo<ContactsModel> ContactServ { get; set; }

        private ContactsModel Tools { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private Modal modal;
        [Parameter]
        public Guid Id { get; set; } = new Guid("80beea30-3f74-42f3-812b-561cea25ec32");


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
