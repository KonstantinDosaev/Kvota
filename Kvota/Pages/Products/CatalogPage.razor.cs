using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Kvota.Pages.Products
{
    partial class CatalogPage
    {
        private IEnumerable<Category>? Categories { get; set; }
        [Parameter]
        public Guid? BrandId { get; set; }
        public Brand? Brand { get; set; }
            MudListItem? _selectedItem;
        [Parameter]
        public object? SelectedValue { get; set; } = 1;
        string _value;

        
        [Parameter]
        public Category? SelectedCategory
        {
            get => (Category?)SelectedValue;
            set => SelectedValue = value;
        }
        [Parameter]
        public EventCallback<Category> SelectedCategoryChanged { get; set; }
        [Parameter]
        public EventCallback<Category> IsSetCategory { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetCategories();
            if (BrandId!=null)
            {
                using var scope = serviceScopeFactory.CreateScope();
                Brand = await scope.ServiceProvider.GetService<IRepo<Brand>>()!.GetOneAsync((Guid)BrandId);
            }
        }

        async Task ChangeValue()
        {
            SelectedCategory = (Category?)SelectedValue;
            await SelectedCategoryChanged.InvokeAsync((Category?)SelectedValue);
            await IsSetCategory.InvokeAsync((Category?)SelectedValue);
        }

        private async Task GetCategories()
        {
            using var scope = serviceScopeFactory.CreateScope();
            Categories = await scope.ServiceProvider.GetService<IRepo<Category>>()!.GetAllAsync();
           Categories = Categories.Where(w => w.ParentId == null);
        }
    }
}
