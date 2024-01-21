using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;

namespace Kvota.Pages.Admin
{
    partial class ContactTool
    {
        private ContactsModel Tools { get; set; } = default!;

        private Modal? modal;

        protected override async Task OnInitializedAsync()
        {
            Tools = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
        }

        private async void SubmitPlayer()
        {
            await ContactSerialize.Serialize($"{Links.RootPath}/{Links.ContactsJson}", Tools);
            modal?.ShowAsync();
        }
        private void AddOperator()
        {
            Tools.CallOperators ??= new List<CallOperator>();
            Tools.CallOperators.Add(new CallOperator() { });
        }
        private void RemoveOperator(CallOperator callOperator)
        {
            Tools.CallOperators!.Remove(callOperator);
            StateHasChanged();
        }
    }
}
