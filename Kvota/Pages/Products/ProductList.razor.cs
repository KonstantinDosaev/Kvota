using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;


namespace Kvota.Pages.Products
{
    partial class ProductList
    {
        private IEnumerable<Product> Products { get; set; } = new List<Product>();

        private static IEnumerable<Product>? _pagedList;

        [Parameter]
        public Guid CategoriesId { get; set; }
        [Parameter]
        public Guid BrandsId { get; set; }

        [Parameter] 
        public string? Category { get; set; } 
        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            if (Category != null)
            {
                Products = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllByIdAsync(CategoriesId);
            }
            else
            {
                Products = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllAsync();
            }


            GetList(Products);
            
           
            
        }

    
        protected async void GetList(IEnumerable<Product> filteredList)
        {
            
                _pagedList = filteredList;
                
        }


    }
}
