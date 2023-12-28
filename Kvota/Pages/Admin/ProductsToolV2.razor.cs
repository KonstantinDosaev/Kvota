using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using ClosedXML.Excel;
using FastExcel;
using Kvota.Constants;
using Kvota.Models.Content;
using Microsoft.JSInterop;
using MudBlazor;

namespace Kvota.Pages.Admin
{
    partial class ProductsToolV2
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
        private int _quantityInPage = 10;
        private int _currentPageCount=10;
        private Home? _home;
        private List<Storage> _storage;

        private bool _visibleStorageDialog;
        protected override async Task OnInitializedAsync()
        {
            //_pagedList = new List<Product>();
            //using var scope = ServiceScopeFactory.CreateScope();
            //ProductService = scope.ServiceProvider.GetService<IRepo<Product>>()!;
            //Products = await ProductService.GetAllAsync();
            //_filteredList = Products;
            //_home = await HomeSerialize.DeSerialize($"{Links.RootPath}/{Links.HomeContentJson}");

            //LoadMore(0);
            if (table != null) await table.ReloadServerData();
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
        private async void OutputExcel(List<Product> products)
        {
            var outputFile = new FileInfo($"{Links.RootPath}/excel/ExcelOutputKvota.xlsx");
            if (outputFile.Exists)
            {
                outputFile.Delete();
            }
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Лист1");
       
         

                var rowData = new List<object[]>();

                var header = await SetColumns();
                rowData.Add(header);

                foreach (var product in products)
                {

                    var row = new List<object?>
                    {product.Name,
                        product.PartNumber,
                        (string?)product.Brand?.Name,
                        product.Category?.Parent?.Name,
                        product.Category?.Name,
                        product.Description,
                        product.Price,
                        product.DayToDelivery,
                        product.SalePrice
                    };
                    for (int i = 9; i < header.Length; i++)
                    {

                        if (product.Storage != null && product.Storage.Select(s => s.Name).Contains(header[i]))
                        {
                            row.Add(product.ProductsInStorage!.FirstOrDefault(f => f.Storage!.Name! == (string)header[i])!.Quantity);
                        }
                        else
                        {
                            row.Add(null);
                        }
                    }
                    rowData.Add(row.ToArray()!);
                }

                worksheet.Cell(1, 1).InsertTable(rowData);

                worksheet.Row(1).Style.Fill.BackgroundColor= XLColor.Green;
            worksheet.Row(2).Style.Fill.BackgroundColor = XLColor.Gray;
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(outputFile.FullName);
      

            var uri = new Uri(NavigationManager.Uri);
            var urlSave = uri.GetLeftPart(UriPartial.Authority);
            var fileName = "ExcelOutputKvota.xlsx";
            var fileURL = $"{urlSave}/excel/ExcelOutputKvota.xlsx";
            var result = JsRuntime.InvokeVoidAsync("triggerFileDownload", fileName, fileURL);
        }
       
        private async Task<object[]> SetColumns()
        {
            List<object> result;
            result= new List<object>()
            {"Наименование",
                "Парт-номер",
                "Бренд",
                "Категория",
                "Подкатегория",
                "Описание",
                "Цена",
                "Дней на доставку",
                "Скидка",
            };
            using var scope = ServiceScopeFactory.CreateScope();
            _storage = (List<Storage>)await scope.ServiceProvider.GetService<IRepo<Storage>>()!.GetAllAsync();
            foreach (var storage in _storage)
            {
                result.Add(storage.Name);
            }

            return result.ToArray();

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
        private string ProductInStorageConverter(List<ProductsInStorage> items)
        {
            var result = "";
            foreach (var productsInStorage in items)
            {
                result += $"{productsInStorage.Storage!.Name}:{productsInStorage.Quantity}|";
            }

            return result;
        }
    }
   
}
