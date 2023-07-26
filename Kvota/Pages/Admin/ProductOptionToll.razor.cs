using BlazorBootstrap;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Pages.Admin
{
    partial class ProductOptionToll
    {
        private Modal? _modalUpdate;
        private Modal? _modalAdd;
        private Modal? _modalError;
        [Inject] public NavigationManager? NavigationManager { get; set; }

        private ProductOption? Item { get; set; }
        public ProductOption? ItemUpdate { get; set; }
        private List<ProductOption>? ItemList { get; set; }
        private List<CategoryOption>? CategoryOptionList { get; set; }
        [Parameter]
        public Guid? Id { get; set; }
        [Parameter]
        public Guid? Idp { get; set; }
        [Parameter]
        public string? Title { get; set; }
        private Guid _tempId;
        protected override async Task OnInitializedAsync()
        {

            ItemList = (List<ProductOption>)await OptionsRepo.GetAllAsync();
            using var scope = serviceScopeFactory.CreateScope();
            CategoryOptionList = (List<CategoryOption>?)await scope.ServiceProvider.GetService<IRepo<CategoryOption>>()!.GetAllAsync();

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

            Item = new ProductOption() { ProductId = (Guid)Idp! };
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

    }
}
