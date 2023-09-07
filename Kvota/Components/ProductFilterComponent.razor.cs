using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components
{
    partial class ProductFilterComponent
    {
        //private IEnumerable<GrandCategory> GrandCategoryList { get; set; } = default!;

        [Parameter]
        public IEnumerable<Product>? Products { get; set; }


        private  IEnumerable<Product>? _filteredList { get; set; }
        private IEnumerable<Brand> BrandList { get; set; }= default!;
        private IEnumerable<Category> CategoryList { get; set; } = default!;
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
                    CategoryList = new List<Category>();
                }
                else
                {
                    if (_filteredList!= null )
                    {
                        BrandList = _filteredList.Where(w => w.BrandId != null).Select(s => s.Brand).Distinct().OrderBy(o => o!.Name)!;
                        CategoryList = _filteredList.Where(w => w.CategoryId != null).Select(s => s.Category).Distinct().OrderBy(o => o!.Name)!; 
                        
                    }
                    else
                    {
                        BrandList = value.Where(w => w.BrandId != null).Select(s => s.Brand).Distinct().OrderBy(o => o!.Name)!;
                        CategoryList = value.Where(w => w.CategoryId != null).Select(s => s.Category).Distinct().OrderBy(o => o!.Name)!;
                       
                    }
                }
            }
            return base.SetParametersAsync(parameters);
        }
        //protected override void OnAfterRender(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        if (BrandsFilterId != null && BrandsFilterId != Guid.Empty)
        //        {
        //            GetList();
        //        }
        //    }


        //}


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
                _filteredList = Products!
                    .Where(x => x.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1 || (x.PartNumber!=null && x.PartNumber.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1))
                    .ToList();
                await ProductListCallback.InvokeAsync(_filteredList);
            }
            if (string.IsNullOrEmpty(SearchString))
            {
                ResetSearch();
                _filteredList = Products!;
                await ProductListCallback.InvokeAsync(_filteredList);
            }

        }
        private async void ResetSearch()
        {
            SearchString = string.Empty;
            _filteredList = Products!;
            await ProductListCallback.InvokeAsync(_filteredList);
        }


       

    }
}

