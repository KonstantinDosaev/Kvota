using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Kvota.Models.Products;
using OfficeOpenXml;
using LicenseContext = System.ComponentModel.LicenseContext;
using Kvota.Interfaces;
using Kvota.Repositories.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Kvota.Pages.Admin
{
    partial class ExcelDownload
    {
        string Message = "не загружен файл";
        private IReadOnlyList<IBrowserFile> _selectedFiles = null!;

        private Guid GrandCategoryIdp { get; set; }
        [Parameter]
        public string? Name { get; set; }
        [Parameter]
        public string? Patch { get; set; }
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }
        private List<string>? _productsName ;
        private List<Brand>? _brandList ;
        private List<GrandCategory>? _grandCategoryList ;
        private List<Category>? _categoryList;
        private bool _update ;
        private IEnumerable<Product> _prodList;
        protected override async Task  OnInitializedAsync()
        {
             _prodList = await ProductService.GetAllAsync();
            _productsName = _prodList.Select(s=>s.Name).ToList();
            _brandList = (List<Brand>?)await BrandService.GetAllAsync();
            _grandCategoryList = (List<GrandCategory>)await GrandCategoryService.GetAllAsync();
            _categoryList = (List<Category>)await CategoryService.GetAllAsync();

        }
        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            _selectedFiles = e.GetMultipleFiles();
            Message = $"{_selectedFiles.Count} загружено";
            this.StateHasChanged();
        }

        private async void OnSubmit()
        {
            foreach (var file in _selectedFiles)
            {

                Stream stream = file.OpenReadStream();
                var path = $"{env.WebRootPath}\\{file.Name}";
                Patch = path;
                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
               // await OnClickCallback.InvokeAsync(PatchImage);
            }

            Message = $"загружено файлов {_selectedFiles.Count}";
            this.StateHasChanged();
        }

        public async Task ReadExcel()
        {
           var orders = new List<Product>();
            var FilePath = Patch;
            FileInfo existingFile = new FileInfo(FilePath);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    var order = new Product();
                    _update = false;
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            if (col == 1)
                            {
                                order.Name = worksheet.Cells[row, col].Value.ToString() ?? throw new InvalidOperationException();
                                
                                if (_productsName != null && _productsName.Contains(order.Name))
                                {
                                    var tempProduct = _prodList.FirstOrDefault(w => w.Name == order.Name)!;
                                    order.Id = tempProduct.Id;
                                    order.DateTimeCreated = tempProduct.DateTimeCreated;
                                    order.Image= tempProduct.Image;
                                    _update=true;
                                }
                            }
                            else if (col == 2)
                            {
                                order.PartNumber = worksheet.Cells[row, col].Value.ToString();
                            }
                            else if (col == 3)
                            {
                                var brand = worksheet.Cells[row, col].Value.ToString();
                                var tempBrand = _brandList.Any() ? _brandList.FirstOrDefault(w => w.Name == brand) : null;
                                if (tempBrand == null)
                                {
                                    tempBrand = new Brand() { Name = brand! };
                                    _brandList.Add(tempBrand);
                                    await BrandService.AddAsync(tempBrand);
                                }
                                order.BrandId = tempBrand!.Id;
                            }
                            else if (col == 4)
                            {
                                var gcategory = worksheet.Cells[row, col].Value.ToString();
                               
                                var tempGrand = _grandCategoryList.Any()? _grandCategoryList.FirstOrDefault(w => w.Name == gcategory):null;
                                if (tempGrand == null && gcategory != null)
                                {
                                    tempGrand = new GrandCategory() { Name = gcategory };
                                    _grandCategoryList.Add( tempGrand);
                                    await GrandCategoryService.AddAsync(tempGrand);
                                }

                                GrandCategoryIdp = tempGrand!.Id;
                            }
                            else if (col == 5)
                            {
                                var category = worksheet.Cells[row, col].Value.ToString();
                                
                                var tempGrand =_categoryList.Any() ? _categoryList.FirstOrDefault(w => w.Name == category) : null;
                                if (tempGrand == null && category != null)
                                {
                                    tempGrand = new Category() { Name = category, GrandCategoryId = GrandCategoryIdp};
                                    _categoryList.Add( tempGrand);
                                    await CategoryService.AddAsync(tempGrand);
                                }

                                order.CategoryId = tempGrand!.Id;
                            }
                            else if (col == 6) order.Description = worksheet.Cells[row, col].Value.ToString();
                            else if (col == 7) order.Price = Math.Round((Convert.ToDecimal(worksheet.Cells[row, col].Value)),2);
                            else if (col == 8) order.Quantity = Convert.ToInt32(worksheet.Cells[row, col].Value);
                            else if (col == 9) order.QuantityTwo = Convert.ToInt32(worksheet.Cells[row, col].Value);
                            //else if (col == 10) order.QuantityTwo = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        }
                    }

                    if (!_update)
                    {
                        order.Image = "image\\products\\default.jpg";
                        order.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                        orders.Add(order);
                    }
                    else
                    {
                        order.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                        using var scope = ServiceScopeFactory.CreateScope();
                        await scope.ServiceProvider.GetService<IRepo<Product>>().Update(order);
                    }
                    
                }
            }

            Console.WriteLine("ff");

            await ProductService.AddRangeAsync(orders);

        }

    
    }
}
