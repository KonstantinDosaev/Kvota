using BlazorBootstrap;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components.Web;

namespace Kvota.Components.Admin.Products
{
    partial class SearchProduct
    {
        private List<Product>? SearchList { get; set; } 
        protected string SearchString { get; set; } = string.Empty;
        private Modal? _modalSearch;
        protected async void SearchProducts()
        {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    SearchList = (List<Product>)await ProductRepo.GetSearch(SearchString);
                    await _modalSearch!.ShowAsync();
                }
               
        }
        protected  void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                if (!string.IsNullOrEmpty(SearchString))
                    SearchProducts();
            }
        }

    }
}
