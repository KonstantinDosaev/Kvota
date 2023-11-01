using System.Text.Json;
using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.JSInterop;

namespace Kvota.Pages
{
    partial class Index
    {
        private List<Brand> BrandList { get; set; } = null!;
        public List<Product> ProductList { get; set; } = null!;
        private Home Content { get; set; } = null!;
        private string[] AboutArr { get; set; } = default!;
        private bool BrandView { get; set; }

        private Modal? _modalProductCard;
        private Guid IdCurrent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Content = await HomeSerialize.DeSerialize($"{Links.RootPath}/{Links.HomeContentJson}");
            BrandView = Content.BrandView;
            AboutArr = Content.AboutHomeText!.Split('^');
            if (BrandView)
            {
                BrandList = (List<Brand>)await BrandRepos.GetAllAsync();
                BrandList = BrandList.Where(w => !string.IsNullOrEmpty(w.Image)).Take(30).ToList();
            }
            ProductList = ((ProductRepo)ProductRepos).GetAllByQuery().Where( w=> Content.ProductInHome.Contains(w.Id)).ToList();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            var t = JsRuntime.InvokeVoidAsync("onHomeReady");
            base.OnAfterRender(firstRender);
        }
        private void GetModalUpdate(Guid id)
        {
            IdCurrent = id;
            _modalProductCard?.ShowAsync();

        }
        private async Task OnHideModalClick()
        {
            await _modalProductCard?.HideAsync()!;
        }
    }
}
