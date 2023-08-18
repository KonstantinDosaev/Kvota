using Kvota.Interfaces;
using Kvota.Models.Content;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;

namespace Kvota.Shared
{
    partial class MainLayout
    {
        private ContactsModel Contact { get; set; } = default!;
        private string buttonDisplay = "none";
        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            Contact = await scope.ServiceProvider.GetService<IRepo<ContactsModel>>()!.GetOneAsync(new Guid("80beea30-3f74-42f3-812b-561cea25ec32"));

            await InvokeAsync(StateHasChanged);
        }
    }
}
