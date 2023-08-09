using BlazorBootstrap;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Pages.Admin
{
    partial class GrandCategoryTool
    {

        private Modal? _modalUpdate;
        private Modal? _modalAdd;
        private Modal? _modalError;
        [Inject] public NavigationManager? NavigationManager { get; set; }

        public GrandCategory? Item { get; set; }
        public GrandCategory? ItemUpdate { get; set; }
        private List<GrandCategory>? ItemList { get; set; }
        
        protected override async Task OnInitializedAsync()
        {

            ItemList = (List<GrandCategory>)await GRepo.GetAllAsync();
            await InvokeAsync(StateHasChanged);
        }


        private async void Delete(Guid id)
        {
            try
            {
                await GRepo.DeleteAsync(id);
                NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            catch
            {
                _modalError?.ShowAsync();
            }

        }

        private void GetModalUpdate(Guid id)
        {
            ItemUpdate = GRepo.GetOne(id);
            _modalUpdate?.ShowAsync();
        }

        private void GetModalAdd()
        {
            Item = new GrandCategory();
            _modalAdd?.ShowAsync();

        }

        private async void SubmitAdd()
        {
            await GRepo.AddAsync(Item);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

        private async void SubmitUpdate()
        {
            await GRepo.Update(ItemUpdate);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);



        }
    }
}
