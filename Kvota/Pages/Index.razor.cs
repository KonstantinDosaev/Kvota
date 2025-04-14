using System.Linq;
using System.Text.Json;
using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Kvota.RepositoriesUi.Products;
using Microsoft.JSInterop;

namespace Kvota.Pages
{
    partial class Index
    {
        private List<Brand> BrandList { get; set; } = null!;
        public List<Product> ProductList { get; set; } = null!;
        public List<Product> SearchingProducts { get; set; } = null!;
        private Home Content { get; set; } = null!;
        private string[] AboutArr { get; set; } = default!;
        private bool BrandView { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Content = await HomeSerialize.DeSerialize($"{Links.RootPath}/{Links.HomeContentJson}");
            BrandView = Content.BrandView;
            AboutArr = Content.AboutHomeText!.Split('^');
            if (BrandView)
            {
                BrandList = BrandRepos.GetAllByQuery().Where(w => !string.IsNullOrEmpty(w.Image)).Take(30).ToList();
            }
            //ProductList = ((ProductRepoUi)ProductRepos).GetAllByQuery().Where( w=> Content.ProductInHome != null && Content.ProductInHome.Contains(w.Id)).OrderBy(o=>o.Name).ToList();
            //SearchingProducts= ((ProductRepoUi)ProductRepos).GetAllByQuery().Where(w => Content.HotSearchProducts != null && Content.HotSearchProducts.Contains(w.Id)).ToList();
            var pr = await (ProductRepos).GetAllAsync();
            if (Content.ProductInHome != null)
                ProductList = (List<Product>)await (ProductRepos).GetAllContainsInIdsAsync(Content.ProductInHome);
            if (Content.HotSearchProducts != null)
                SearchingProducts = (List<Product>)await (ProductRepos).GetAllContainsInIdsAsync(Content.HotSearchProducts);
            SearchingProducts.Reverse(0, SearchingProducts.Count);
        }
        //protected override void OnAfterRender(bool firstRender)
        //{
        //    var t = JsRuntime.InvokeVoidAsync("onHomeReady");
        //    base.OnAfterRender(firstRender);
        //}
 
    }
}
