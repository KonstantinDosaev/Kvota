using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using ClosedXML.Excel;
using FastExcel;
using Kvota.Constants;
using Kvota.Models.Content;
using Microsoft.JSInterop;
using MudBlazor;
using DocumentFormat.OpenXml.Spreadsheet;
using Kvota.Repositories.Products;

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
        private Home? _home;
        private List<Storage> _storage;

        private bool _visibleStorageDialog;
        bool _visibleCreatedProductDialog;
        bool _visibleEditProductDialog;
        public Guid ProductId;

        private int _quantityInPage = 10;
        private int _currentPageCount = 10;
        private int _totalItems;
        private int _paqeIndex = 0;
        private string? _title;
        private string? _searchString;
        private string _sortString = SortLabelsProduct.NameField;
        private SortDirection _sortDirection = SortDirection.Descending;
        private FilterProductTuple? _filterProductTuple;
        private bool _visibleProductFilter;
        private bool _getHotProduct;

        protected override async Task OnInitializedAsync()
        {
            _home = await HomeSerialize.DeSerialize($"{Links.RootPath}/{Links.HomeContentJson}");
            await InitList();
        }
        
        private async Task<List<Product>> ServerReload()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetService<IRepo<Product>>();
            var response = await ((ProductRepo)productService!).GetBySortPagedSearchChapterAsync(new SortPagedRequest<FilterProductTuple>()
            {
                PageIndex = _paqeIndex,
                PageSize = _quantityInPage,
                SearchString = _searchString,
                SortLabel = _sortString,
                SortDirection = _sortDirection,
                FilterTuple = _filterProductTuple

            });
            //  pagedData = response.Items;

            _totalItems = response.TotalItems;
            return (response.Items ?? Array.Empty<Product>()).ToList();
        }

        private async Task InitList()
        {
            _paqeIndex = 0;
            _pagedList = await ServerReload();
        }

        private async Task LoadMore()
        {
            if (_pagedList!.Count == _totalItems)
                return;
            _paqeIndex += 1;
            _pagedList!.AddRange(await ServerReload());
        }
        private async void DeleteCheckedProduct(List<Guid> ids)
        {
            //foreach (var id in ids)
            //{
            //    DeleteImage(id);
            //}
            await ProductRepo.DeleteRangeAsync(ids);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        private void OpenUpdateProductDialog(Guid id)
        {
            ProductId = id;
            _visibleEditProductDialog = true;
        }


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

        protected async Task OutputAllToExcel()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetService<IRepo<Product>>();
            var productList = (List<Product>)(await productService!.GetAllAsync());
            if (productList.Any())
                OutputExcel(productList);
        }
        protected void OutputViewToExcel()
        {
            if (_pagedList != null) OutputExcel(_pagedList);
        }
        protected async Task OutputFilteredListToExcel()
        {
            if (_filterProductTuple == null && string.IsNullOrEmpty(_searchString))
            {
                _snackBar.Add("Не установлены фильтры");
                return;
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetService<IRepo<Product>>();
            var response = await ((ProductRepo)productService!).GetBySortPagedSearchChapterAsync(new SortPagedRequest<FilterProductTuple>()
            {
                PageIndex = 0,
                PageSize = _totalItems,
                SearchString = _searchString,
                SortLabel = _sortString,
                SortDirection = _sortDirection,
                FilterTuple = _filterProductTuple
            });
            
            var productList = response.Items;
            if (productList != null) 
                OutputExcel(productList.ToList());
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

            var uri = new Uri(NavigationManager!.Uri);
            var urlSave = uri.GetLeftPart(UriPartial.Authority);
            var fileName = "ExcelOutputKvota.xlsx";
            var fileURL = $"{urlSave}/excel/ExcelOutputKvota.xlsx";
            var result = JsRuntime.InvokeVoidAsync("triggerFileDownload", fileName, fileURL);
            if (result.IsCompletedSuccessfully)
            {
                _snackBar.Add("Файл создан");
            }
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
            if (_home!.ProductInHome != null && _home.ProductInHome.Contains(id)) return;
            _home.ProductInHome ??= new List<Guid>();
            _home.ProductInHome.Add(id);
            await HomeSerialize.Serialize($"{Links.RootPath}/{Links.HomeContentJson}", _home);
        }
        private async void RemoveProductFromHome(Guid id)
        {
            if (_home!.ProductInHome == null) return;
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

        private async Task OnSortDirection()
        {
            _paqeIndex = 0;
            _sortDirection = _sortDirection == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            _pagedList = await ServerReload();
        }
        private async Task OnSorted()
        {
            _paqeIndex = 0;
            _pagedList = await ServerReload();
        }
        private async Task OnSearch(string text)
        {
            _paqeIndex = 0;
            _searchString = text;
            _pagedList = await ServerReload();
        }
        private async Task OnFilter(FilterProductTuple filterProductTuple)
        {
            _paqeIndex = 0;
            _filterProductTuple = filterProductTuple;
            _pagedList = await ServerReload();
        }
        private async Task GetHotProduct()
        {
            _getHotProduct = _getHotProduct != true;
            if (_getHotProduct)
            {
                using var scope = ServiceScopeFactory.CreateScope();
                var productService = scope.ServiceProvider.GetService<IRepo<Product>>();
                if (_home is { ProductInHome: { } } && _home.ProductInHome.Any())
                    _filterProductTuple = new FilterProductTuple()
                    {
                        AllProductsToGuid = _home.ProductInHome
                    };
                var response = await ((ProductRepo)productService!).GetBySortPagedSearchChapterAsync(new SortPagedRequest<FilterProductTuple>()
                {
                    PageIndex = 0,
                    PageSize = _quantityInPage,
                    SortLabel = _sortString,
                    SortDirection = _sortDirection,
                    FilterTuple = _filterProductTuple

                });
                _totalItems = response.TotalItems;
                _pagedList = (List<Product>?)response.Items;
            }
            else
            {
                _filterProductTuple = null;
                _pagedList = await ServerReload();
            }
           
        }
    }
   
}
