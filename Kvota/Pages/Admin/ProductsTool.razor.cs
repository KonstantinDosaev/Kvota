using BlazorBootstrap;
using FastExcel;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.JSInterop;
using NuGet.Packaging;

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
        private static IEnumerable<GrandCategory>? GCategoryList { get; set; }
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
            GCategoryList = await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
            Products = await ProductService.GetAllAsync();
            _filteredList = Products;
            rootPath = Env.WebRootPath;
            LoadMore(0);
           // GetPagedList(_filteredList);
           // await InvokeAsync(StateHasChanged);
        }
        //private async void DeleteProduct(Guid id,string pathImage)
        //{
        //    var path = $"{Env.WebRootPath}\\{pathImage}";
        //    var fileInf = new FileInfo(path);
        //    if (fileInf.Exists)
        //    {
        //        fileInf.Delete();
        //    }
        //    await ProductRepo.DeleteAsync(id);
        //    NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        //}
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
        private  void OutputExcel(List<Product> products)
        {
            var outputFile = new FileInfo($"{Env.WebRootPath}\\excel\\ExcelOutputKvota.xlsx");
            List<MyObject> objectList = new List<MyObject>();

                foreach (var item in products)
                {
                    MyObject genericObject = new MyObject();
                    genericObject.NameColumn = item.Name;
                    if (item.PartNumber != null) genericObject.PartColumn = item.PartNumber;
                    if (item.Brand != null) genericObject.BrandColumn = item.Brand.Name;
                    if (item.Category != null)
                        if (item.Category.GrandCategoryId != null)
                            genericObject.GCategotyColumn = GCategoryList!.FirstOrDefault(w => w.Id==item.Category.GrandCategoryId)!.Name;
                    if (item.Category != null) genericObject.CategoryColumn = item.Category.Name;
                    genericObject.DescriptionColumn = item.Description;
                    genericObject.PriceColumn = item.Price;
                    genericObject.QuntityColumn = item.Quantity;
                    genericObject.TwoQuntityColumn = item.QuantityTwo;
                    genericObject.DateDeleveyColumn = item.DateDelivery;
                    objectList.Add(genericObject);
                }
          
                var workbook = new XLWorkbook();
                var wsDetailedData = workbook.AddWorksheet("data");
                wsDetailedData.Cell(1, 1).InsertTable(objectList);
                workbook.SaveAs(outputFile.FullName);
                var uri = new Uri(NavigationManager.Uri);
                var urlSave = uri.GetLeftPart(UriPartial.Authority);
                var fileName = "ExcelOutputKvota.xlsx";
                var fileURL = $"{urlSave}/excel/ExcelOutputKvota.xlsx";
               var result =  JsRuntime.InvokeVoidAsync("triggerFileDownload", fileName, fileURL);


        }
    }
    record MyObject
    {
        public string NameColumn { get; set; }
        public string PartColumn { get; set; }
        public string BrandColumn { get; set; }
        public string GCategotyColumn { get; set; }
        
        public string CategoryColumn { get; set; }
        public string? DescriptionColumn { get; set; }
        public decimal? PriceColumn { get; set; }
        public int? QuntityColumn { get; set; }
        public int? TwoQuntityColumn { get; set; }
        public DateOnly? DateDeleveyColumn { get; set; }
    }
}
