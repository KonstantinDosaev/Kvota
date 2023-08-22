using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Kvota.Models.Products;
using Kvota.Interfaces;
using FastExcel;
using System.Globalization;


namespace Kvota.Pages.Admin
{
    partial class ExcelDownload
    {
        string Message = "не загружен файл";
        private IBrowserFile _selectedFiles = null!;

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
            _selectedFiles = e.File;
            this.StateHasChanged();
        }

        private async void OnSubmit()
        {
            Stream stream = _selectedFiles.OpenReadStream();
                var path = $"{env.WebRootPath}\\{_selectedFiles.Name}";
                Patch = path;
                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
                Message = "загружено";
            this.StateHasChanged();
        }

        public async Task ReadExcel()
        {
            var orders = new List<Product>();
            var FilePath = Patch;
            var existingFile = new FileInfo(FilePath);
            if (!existingFile.Exists)
            {
                throw new FileNotFoundException("The file " + FilePath + " not exist");
            }


            using (FastExcel.FastExcel fastExcel = new FastExcel.FastExcel(existingFile, true))
            {
                Worksheet worksheet = fastExcel.Read(1);
                var colCount = worksheet.Rows.FirstOrDefault()!.Cells.Count();
                var rowCount = worksheet.Rows.Count();
            

                worksheet.Read();
                Row[] rows = worksheet.Rows.ToArray();

                foreach (var item in rows)
                {
                    if (item.RowNumber==1)
                        continue;
                    
                    var order = new Product();
                    _update = false;

                    foreach (var cellItem in item.Cells)
                    {
                        if (cellItem.Value != null)
                        {
                            if (cellItem.ColumnNumber == 1)
                            {
                                order.Name = cellItem.Value.ToString() ?? throw new InvalidOperationException();

                                if (_productsName != null && _productsName.Contains(order.Name))
                                {
                                    var tempProduct = _prodList.FirstOrDefault(w => w.Name == order.Name)!;
                                    order.Id = tempProduct.Id;
                                    order.DateTimeCreated = tempProduct.DateTimeCreated;
                                    order.Image = tempProduct.Image;
                                    _update = true;
                                }
                            }
                            else if (cellItem.ColumnNumber == 2)
                            {
                                order.PartNumber = cellItem.Value.ToString();
                            }
                            else if (cellItem.ColumnNumber == 3)
                            {
                                var brand = cellItem.Value.ToString();
                                var tempBrand = _brandList.Any() ? _brandList.FirstOrDefault(w => w.Name == brand) : null;
                                if (tempBrand == null)
                                {
                                    tempBrand = new Brand() { Name = brand! };
                                    _brandList.Add(tempBrand);
                                    await BrandService.AddAsync(tempBrand);
                                }
                                order.BrandId = tempBrand!.Id;
                            }
                            else if (cellItem.ColumnNumber == 4)
                            {
                                var gcategory = cellItem.Value.ToString();

                                var tempGrand = _grandCategoryList.Any() ? _grandCategoryList.FirstOrDefault(w => w.Name == gcategory) : null;
                                if (tempGrand == null && gcategory != null)
                                {
                                    tempGrand = new GrandCategory() { Name = gcategory };
                                    _grandCategoryList.Add(tempGrand);
                                    await GrandCategoryService.AddAsync(tempGrand);
                                }

                                GrandCategoryIdp = tempGrand!.Id;
                            }
                            else if (cellItem.ColumnNumber == 5)
                            {
                                var category = cellItem.Value.ToString();

                                var tempGrand = _categoryList.Any() ? _categoryList.FirstOrDefault(w => w.Name == category) : null;
                                if (tempGrand == null && category != null)
                                {
                                    tempGrand = new Category() { Name = category, GrandCategoryId = GrandCategoryIdp };
                                    _categoryList.Add(tempGrand);
                                    await CategoryService.AddAsync(tempGrand);
                                }

                                order.CategoryId = tempGrand!.Id;
                            }
                            else if (cellItem.ColumnNumber == 6) order.Description = cellItem.Value.ToString();
                            else if (cellItem.ColumnNumber == 7) order.Price = Math.Round((Convert.ToDecimal(cellItem.Value,CultureInfo.InvariantCulture)), 2);
                            else if (cellItem.ColumnNumber == 8) order.Quantity = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture));
                            else if (cellItem.ColumnNumber == 9) order.QuantityTwo = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture));
                            //else if (col == 10) order.QuantityTwo = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        }
                    }

                    if (!_update)
                    {
                        order.Image = "image\\products\\default.jpg";
                        order.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                        order.DateTimeUpdated = order.DateTimeCreated;
                        orders.Add(order);
                    }
                    else
                    {
                        order.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                        using var scope = ServiceScopeFactory.CreateScope();
                        await scope.ServiceProvider.GetService<IRepo<Product>>()!.Update(order);
                    }

                }
            }


            if (orders.Count!=0)
            {
                await ProductService.AddRangeAsync(orders);
            }
            File.Delete(Patch!);
            Message = "Изменения в каталог внесены";
           
        }


    }
}
