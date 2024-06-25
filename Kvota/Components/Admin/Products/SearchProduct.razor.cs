using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;
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
   
        protected async Task SearchProducts()
        {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    NavigationManager!.NavigateTo($"/search/{SearchString}", forceLoad: true);
                }
               
        }
        protected async  void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                if (!string.IsNullOrEmpty(SearchString))
                    await SearchProducts();
            }
        }
       

    }
}
