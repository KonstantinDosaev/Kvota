using BlazorBootstrap;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace Kvota.Components.Admin.Products
{
    partial class SearchProduct
    {
      
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        protected string SearchString { get; set; } = string.Empty;
        protected  void SearchProducts()
        {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    NavigationManager!.NavigateTo($"/search/{SearchString}", forceLoad: true);
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
