using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using ClosedXML.Excel;
using Kvota.Constants;
using Kvota.Models.Content;
using Microsoft.JSInterop;

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
        private int _quantityInPage = 10;
        private int _currentPageCount=10;
        private Home? _home;
        protected override async Task OnInitializedAsync()
        {
            _pagedList = new List<Product>();
            using var scope = ServiceScopeFactory.CreateScope();
            ProductService = scope.ServiceProvider.GetService<IRepo<Product>>()!;
            GCategoryList = await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
            Products = await ProductService.GetAllAsync();
            _filteredList = Products;
            _home = await HomeSerialize.DeSerialize($"{Links.RootPath}/{Links.HomeContentJson}");

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
            var pathImage = Products!.Where(w=>w.Id==id).Select(s=>s.Image).FirstOrDefault();
            if (pathImage == Links.DefaultImageProduct) return;
            var path = $"{Links.RootPath}/{pathImage}";
            var dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
            }
        }

        private  void LoadMore(int newPageNumber)
        {
            _currentPageCount = newPageNumber;
            _pagedList!.AddRange((_filteredList!.Skip((_currentPageCount)).Take(_quantityInPage)).ToList());

        }
        private  void OutputExcel(List<Product> products)
        {
            var outputFile = new FileInfo($"{Links.RootPath}/excel/ExcelOutputKvota.xlsx");
            var objectList = new List<MyObject>();

                foreach (var item in products)
                {
                    var genericObject = new MyObject();
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
                    genericObject.DateDeleveyColumn = item.DayToDelivery;
                    genericObject.SalePrice = item.SalePrice;
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
        private async void AddProductToHome(Guid id)
        {
            if (_home.ProductInHome.Contains(id)) return;
            _home.ProductInHome ??= new List<Guid>();
            _home.ProductInHome.Add(id);
            await HomeSerialize.Serialize($"{Links.RootPath}/{Links.HomeContentJson}", _home);
        }
        private async void RemoveProductFromHome(Guid id)
        {
            if (_home.ProductInHome == null) return;
            _home.ProductInHome.Remove(id);
              
            await HomeSerialize.Serialize($"{Links.RootPath}/{Links.HomeContentJson}", _home);

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
        public int? DateDeleveyColumn { get; set; }
        public decimal? SalePrice { get; set; }
    }
}
