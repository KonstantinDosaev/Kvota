using BlazorBootstrap;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Kvota.Pages.Admin
{
    partial class CategoryOptionsTool
    {
        private Modal? _modalUpdate;
        private Modal? _modalAdd;
        private Modal? _modalError;
        [Inject] public NavigationManager? NavigationManager { get; set; }

        private CategoryOption? Item { get; set; }
        public CategoryOption? ItemUpdate { get; set; }
        private List<CategoryOption>? ItemList { get; set; }
        private List<Category>? CategoryList { get; set; }
        [Parameter]
        public Guid? Id { get; set; }
        [Parameter]
        public string? Title { get; set; }
        private Guid _tempId;
        protected override async Task OnInitializedAsync()
        {

            ItemList = (List<CategoryOption>)await OptionsRepo.GetAllAsync();
            using var scope = serviceScopeFactory.CreateScope();
            CategoryList = (List<Category>?)await scope.ServiceProvider.GetService<IRepo<Category>>()!.GetAllAsync();
            await InvokeAsync(StateHasChanged);

        }


        private async void Delete(Guid id)
        {
            try
            {
                await OptionsRepo.DeleteAsync(id);
                NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            catch
            {
                _modalError?.ShowAsync();
            }

        }

        private async void GetModalUpdate(Guid id)
        {

            ItemUpdate = await OptionsRepo.GetOneAsync(id);
            _modalUpdate?.ShowAsync();
        }

        private void GetModalAdd()
        {

            Item = new CategoryOption() { CategoryId = (Guid)Id! };
            _modalAdd?.ShowAsync();

        }

        private async void SubmitAdd()
        {
            await OptionsRepo.AddAsync(Item);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

        private async void SubmitUpdate()
        {
            await OptionsRepo.Update(ItemUpdate);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);

        }
        private async void SubmitUpdateGrand()
        {
            ItemUpdate!.CategoryId = _tempId;
            await OptionsRepo.Update(ItemUpdate);

        }
    }
}
