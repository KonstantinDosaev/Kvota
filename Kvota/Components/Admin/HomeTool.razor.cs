using Microsoft.AspNetCore.Components;
using System.Numerics;
using Kvota.Interfaces;
using Kvota.Models.Content;
using Kvota.Repositories;
using BlazorBootstrap;

namespace Kvota.Components.Admin
{
    partial class HomeTool
    {
        [Inject]
        protected IRepo<Home> HomeServ { get; set; }
        private Home Tools { get; set; }

        private Modal? modal;


        protected override async Task OnInitializedAsync()
        {
            Tools = await HomeServ.GetOneAsync(new Guid("7c7ed7ca-fda1-4449-8637-c403c61957eb"));
           
        }

        private async void SubmitPlayer()
        {
            await HomeServ.Update(Tools);
            modal?.ShowAsync();

        }
    }
}
