using BlazorBootstrap;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;


namespace Kvota.Pages.Admin
{
    partial class CategoryTool
    {
        private Modal? _modalUpdate;
        private Modal? _modalAdd;
        private Modal? _modalError;
        [Inject] public NavigationManager? NavigationManager { get; set; }
        
        private Category? Item { get; set; }
        public Category? ItemUpdate { get; set; }
        private List<Category>? ItemList { get; set; }
        private List<GrandCategory>? GcList { get; set; }
        [Parameter] 
        public  Guid? Id { get; set; }
        [Parameter]
        public string? Title { get; set; }
        private Guid _tempId;
        protected override async Task OnInitializedAsync()
        {

            ItemList = (List<Category>)await CategoryRepo.GetAllAsync();
            using var scope = serviceScopeFactory.CreateScope();
            GcList = (List<GrandCategory>?)await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
            await InvokeAsync(StateHasChanged);

        }


        private async void Delete(Guid id)
        {
            try
            {
                await CategoryRepo.DeleteAsync(id);
                NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            catch
            {
                _modalError?.ShowAsync();
            }

        }

        private async void GetModalUpdate(Guid id)
        {
           
            ItemUpdate = await CategoryRepo.GetOneAsync(id);
            _modalUpdate?.ShowAsync();
        }

        private void GetModalAdd()
        {

            Item = new Category { GrandCategoryId = Id };
            _modalAdd?.ShowAsync();

        }

        private async void SubmitAdd()
        {
            await CategoryRepo.AddAsync(Item);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

        private async void SubmitUpdate()
        {
            await CategoryRepo.Update(ItemUpdate);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);

        }
        private async void SubmitUpdateGrand()
        {
            ItemUpdate!.GrandCategoryId = _tempId;
            await CategoryRepo.Update(ItemUpdate);

        }

    }
}
