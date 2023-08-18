using BlazorBootstrap;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Kvota.Pages.Admin
{
    partial class ProductsTool
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        private IEnumerable<Product>? Products { get; set; }
        protected IRepo<Product> ProductService { get; set; } = default!;
        private static List<Product>? _pagedList ;
        private static IEnumerable<Product>? _filteredList;
        [Parameter]
        public Guid CategoriesFilterId { get; set; }
        [Parameter]
        public Guid BrandsFilterId { get; set; }

        private Guid _value ;
        private bool _checked;
        public List<Guid> SelectedValues = new();
        private string rootPath;

 
        private int _quantityInPage = 10;
        private int _currentPageCount=10;

        protected override async Task OnInitializedAsync()
        {
            _pagedList = new List<Product>();
            using var scope = ServiceScopeFactory.CreateScope();
            ProductService = scope.ServiceProvider.GetService<IRepo<Product>>()!;
            Products = await ProductService.GetAllAsync();
            _filteredList = Products;
            rootPath = Env.WebRootPath;
            LoadMore(0);
           // GetPagedList(_filteredList);
           // await InvokeAsync(StateHasChanged);
        }
        private async void DeleteProduct(Guid id,string pathImage)
        {
            var path = $"{Env.WebRootPath}\\{pathImage}";
            var fileInf = new FileInfo(path);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
            await ProductRepo.DeleteAsync(id);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        private async void DeleteCheckedProduct(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                DeleteImage(id);
            }
            await ProductRepo.DeleteRangeAsync(ids);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        protected  void GetFilterList(IEnumerable<Product> filteredList)
        {
           _filteredList= filteredList;
           _pagedList= new List<Product>();
           LoadMore(0);

        }
        //protected void GetPagedList(IEnumerable<Product> paginationList)
        //{
        //    _pagedList = paginationList;
        //    StateHasChanged();
        //}
        private void CheckboxClicked(Guid aSelectedId, object aChecked)
        {
            if ((bool)aChecked)
            {
                if (!SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Add(aSelectedId);
                }
            }
            else
            {
                if (SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Remove(aSelectedId);
                }
            }
            //StateHasChanged();
        }

        private void DeleteImage(Guid id)
        {
            var pathImage = Products.Where(w=>w.Id==id).Select(s=>s.Image).FirstOrDefault();
            if (pathImage == "image\\products\\default.jpg") return;
            var path = $"{Env.WebRootPath}\\{pathImage}";
            var dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
            }
        }

        private  void LoadMore(int newPageNumber)
        {
            _currentPageCount = newPageNumber;
            _pagedList.AddRange((_filteredList.Skip((_currentPageCount)).Take(_quantityInPage)).ToList());

        }
    }
}
