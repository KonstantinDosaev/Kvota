using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components
{
    partial class ProductFilterComponent
    {
        private IEnumerable<GrandCategory> GrandCategoryList { get; set; } = default!;
        [Parameter]
        public IEnumerable<Product>? Products { get; set; }

        private static IEnumerable<Product> _filteredList = default!;
        private IEnumerable<Brand> BrandList { get; set; }= default!;
        //private string _sortString = "";



        [Parameter]
        public Guid CategoriesFilterId { get; set; }
        [Parameter]
        public Guid BrandsFilterId { get; set; }

        [Parameter] 
        public string? Category { get; set; } 
        protected string SearchString { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<IEnumerable<Product>> ProductListCallback { get; set; }


        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.TryGetValue<IEnumerable<Product>>(nameof(Products), out var value))
            {
                if (value is null )
                {
                    BrandList = new List<Brand>();
                }
                else
                {
                    BrandList = value.Where(w => w.BrandId != null).Select(s => s.Brand).Distinct().OrderBy(o => o!.Name)!;
                }
            }
            return base.SetParametersAsync(parameters);
        }
        protected override async Task OnInitializedAsync()
        {
           
            using var scope = ServiceScopeFactory.CreateScope();
            GrandCategoryList = await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
            //BrandList = Products.Where(w=>w.BrandId!=null).Select(s => s.Brand).Distinct().OrderBy(o => o!.Name)!;
          
            
        }

        protected async void GetList()
        {
            _filteredList = Products;

            if (CategoriesFilterId != Guid.Empty)
            {
                _filteredList = _filteredList
                    .Where(x => x.CategoryId == CategoriesFilterId)
                    .ToList();
            }
            if (BrandsFilterId != Guid.Empty)
            {
                _filteredList = _filteredList
                    .Where(x => x.BrandId == BrandsFilterId)
                    .ToList();
            }

            //if (_sortString != string.Empty)
            //{
            //    _filteredList = _sortString switch
            //    {
            //        "name" => _filteredList.OrderBy(o => o.Name).ToList(),
            //        "nameDesc" => _filteredList.OrderByDescending(o => o.Name).ToList(),
            //        "dateNews" => _filteredList.OrderByDescending(o => o.DateTimeUpdated).ToList(),
            //        "priceMin" => _filteredList.OrderByDescending(o => o.Price).ToList(),
            //        "priceMax" => _filteredList.OrderBy(o => o.Price).ToList(),
            //        _ => _filteredList
            //    };
            //}


            await ProductListCallback.InvokeAsync(_filteredList);
        }

        private async void Search()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                _filteredList = Products
                    .Where(x => x.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                    .ToList();
                await ProductListCallback.InvokeAsync(_filteredList);
            }
            if (string.IsNullOrEmpty(SearchString))
            {
                ResetSearch();
            }

        }
        private void ResetSearch()
        {
            SearchString = string.Empty;
            _filteredList = Products;
        }


       

    }
}

