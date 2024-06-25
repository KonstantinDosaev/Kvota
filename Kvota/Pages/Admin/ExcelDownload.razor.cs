using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Kvota.Models.Products;
using Kvota.Interfaces;
using FastExcel;
using System.Globalization;
using Kvota.Constants;
using Kvota.Repositories.Products;
using System;


namespace Kvota.Pages.Admin
{
    partial class ExcelDownload
    {
        string Message = "не загружен файл";
        private IBrowserFile _selectedFiles = null!;
        [Inject] private NavigationManager _navigationManager { get; set; }
        private Guid GrandCategoryIdp { get; set; }
        [Parameter]
        public string? Name { get; set; }
        [Parameter]
        public string? Patch { get; set; }
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }
        private List<string>? _productsName ;
        private List<Brand>? _brandList ;
        private List<Category>? _categoryList;
        private bool _update ;
        private IEnumerable<Product>? _prodList;

        private List<Storage> Storages { get; set; }
        private List<Cell>? storageInTable;
        private Guid _mainStorageId;
        private List<string> _errors = new();
        protected override async Task  OnInitializedAsync()
        {
             _prodList = await ProductService.GetAllAsync();
            _productsName = _prodList.Select(s=>s.Name).ToList();
            _brandList = (List<Brand>?)await BrandService.GetAllAsync();
            _categoryList = (List<Category>)await CategoryService.GetAllAsync();
            Storages = (List<Storage>)await StorageRepo.GetAllAsync();

        }
        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            _selectedFiles = e.File;
            this.StateHasChanged();
            _errors.Clear();
        }

        private async void OnSubmit()
        {
            var stream = _selectedFiles.OpenReadStream();
                var path = $"{Links.RootPath}/{_selectedFiles.Name}";
                Patch = path;
                var fs = File.Create(path);
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
                Message = "загружено";
            this.StateHasChanged();
        }
       
        public async Task ReadExcel()
        {
            if (string.IsNullOrEmpty(Patch))
            {
                _snackBar.Add("Не загружен файл Excel");
                return;
            }
        
            var products = new List<Product>();
            var productsToUpdate = new List<Product>();
            var FilePath = Patch;
            var existingFile = new FileInfo(FilePath!);
            if (!existingFile.Exists)
            {
                throw new FileNotFoundException("The file " + FilePath + " not exist");
            }


            using (var fastExcel = new FastExcel.FastExcel(existingFile, true))
            {
                var worksheet = fastExcel.Read(1);

                worksheet.Read();
                Row[] rows = worksheet.Rows.ToArray();
                
                foreach (var item in rows)
                {
                    if (item.RowNumber == 1)
                        continue;
                    
                    if (item.RowNumber == 2)
                    { 
                        storageInTable = item.Cells.Where(w => w.ColumnNumber >= 10).ToList();
                        continue;
                    }
                        
                    
                    var product = new Product();
                    _update = false;
                    try
                    {
                        foreach (var cellItem in item.Cells)
                        {
                            if (cellItem.Value != null)
                            {
                                if (cellItem.ColumnNumber==1)
                                {
                                    product.Name = cellItem.Value.ToString() ?? throw new InvalidOperationException();

                                    if (_productsName != null && _productsName.Contains(product.Name))
                                    {
                                        var tempProduct = _prodList!.FirstOrDefault(w => w.Name == product.Name)!;
                                        product = tempProduct;
                                        _update = true;
                                    }
                                }
                                else if (cellItem.ColumnNumber == 2)
                                {
                                    product.PartNumber = cellItem.Value.ToString();
                                }
                                else if (cellItem.ColumnNumber == 3)
                                {
                                    var brand = cellItem.Value.ToString()!.Trim();
                                    var tempBrand = _brandList!.Any() ? _brandList!.FirstOrDefault(w => string.Equals(w.Name, brand, StringComparison.CurrentCultureIgnoreCase)) : null;
                                    if (tempBrand == null)
                                    {
                                        tempBrand = new Brand() { Name = brand! };
                                        _brandList!.Add(tempBrand);
                                        await BrandService.AddAsync(tempBrand);
                                    }
                                    product.BrandId = tempBrand!.Id;
                                }
                                else if (cellItem.ColumnNumber == 4)
                                {
                                    var gcategory = cellItem.Value.ToString()!.Trim();

                                    if (string.IsNullOrEmpty(gcategory))
                                        continue;

                                    var tempGrand = _categoryList!.Any() ? _categoryList!.FirstOrDefault(w => string.Equals(w.Name, gcategory, StringComparison.CurrentCultureIgnoreCase)) : null;
                                    if (tempGrand == null)
                                    {
                                        tempGrand = new Category() { Name = gcategory };
                                        _categoryList!.Add(tempGrand);
                                        await CategoryService.AddAsync(tempGrand);
                                    }
                                    else
                                    {
                                        if (tempGrand.Products != null && tempGrand.Products.Any())
                                            continue;

                                    }
                                    GrandCategoryIdp = tempGrand!.Id;
                                }
                                else if (cellItem.ColumnNumber == 5)
                                {
                                    var category = cellItem.Value.ToString()!.Trim();
                                    if (string.IsNullOrEmpty(category))
                                        continue;

                                    var tempCategory = _categoryList!.Any() ? _categoryList!.FirstOrDefault(w => string.Equals(w.Name, category, StringComparison.CurrentCultureIgnoreCase)) : null;
                                    if (tempCategory == null)
                                    {
                                        tempCategory = new Category() { Name = category, ParentId = GrandCategoryIdp };
                                        _categoryList!.Add(tempCategory);
                                        await CategoryService.AddAsync(tempCategory);
                                    }
                                    else
                                    {
                                        if (tempCategory.Children != null && tempCategory.Children.Any())
                                        {
                                            var newCategory = new Category() { Id = Guid.NewGuid(), Name = category + 1, ParentId = null };
                                            _categoryList!.Add(newCategory);
                                            await CategoryService.AddAsync(newCategory);
                                            product.CategoryId = newCategory!.Id;
                                            continue;
                                        }
                                    }

                                    product.CategoryId = tempCategory!.Id;
                                }
                                else if (cellItem.ColumnNumber == 6) product.Description = cellItem.Value.ToString();
                                else if (cellItem.ColumnNumber == 7) product.Price = Math.Round((Convert.ToDecimal(cellItem.Value, CultureInfo.InvariantCulture)), 2);
                                else if (cellItem.ColumnNumber == 8) product.DayToDelivery = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture));
                                else if (cellItem.ColumnNumber == 9) product.SalePrice = Math.Round((Convert.ToDecimal(cellItem.Value, CultureInfo.InvariantCulture)), 2);
                                
                            }
                            if (cellItem.ColumnNumber >= 10)
                            {
                                var storage = Storages.FirstOrDefault(f =>
                                    f.Name == storageInTable!
                                        .FirstOrDefault(f => f.ColumnNumber == cellItem.ColumnNumber)!.Value
                                        .ToString());
                                if (storage == null)
                                {
                                    _errors.Add($"склад {storageInTable!.FirstOrDefault(f => f.ColumnNumber == cellItem.ColumnNumber)!.Value} не найден");
                                    continue;
                                }
                                product.ProductsInStorage ??= new List<ProductsInStorage>();

                                var productInStorage =
                                    product.ProductsInStorage.FirstOrDefault(f => f.Storage == storage);
                                if (productInStorage != null)
                                {
                                    productInStorage.Quantity = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    product.ProductsInStorage.Add(new ProductsInStorage() { ProductId = product.Id, StorageId = storage.Id, Quantity = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture)) });
                                }



                            }
                        }

                        if (!_update)
                        {
                            product.Image = Links.DefaultImageProduct;
                            product.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                            product.DateTimeUpdated = product.DateTimeCreated;
                            products.Add(product);
                        }
                        else
                        {
                            product.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                            await ProductService.ManualSaveAsync();

                        }
                    }
                    catch (Exception e)
                    {
                        _errors.Add($"Ошибка: проверьте правильность ячейки количества в продукте :---> {product.Name}");
                    }
                   

                }
            }


            if (products.Count!=0)
            {
                await ProductService.AddRangeAsync(products);
            }
            
            File.Delete(Patch!);
            if (!_errors.Any())
            {
                _navigationManager.NavigateTo("/admin/product/", forceLoad: true);
                _snackBar.Add("Загрузка произведена успешно");
            }
           
         
            
           
        }


    }
}
