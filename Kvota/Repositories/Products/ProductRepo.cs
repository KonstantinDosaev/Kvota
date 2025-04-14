using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Kvota.Constants;
using Kvota.Migrations;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MudBlazor;

namespace Kvota.Repositories.Products
{
    public class ProductRepo:BaseRepo<Product>
    {
        public ProductRepo(KvotaProductContext context) : base(context)
        {
            _productOptions = context.ProductOptions;
            Table = context.Products;
            Context = context;
            _productsInStorages = context.ProductsInStorages;

        }

        private readonly DbSet<ProductsInStorage> _productsInStorages;
        private readonly DbSet<ProductOption> _productOptions;
        public override async Task<IEnumerable<Product>> GetAllAsync() => await Table.OrderBy(o => o.Name)
            .Include(i => i.Brand).Include(i => i.Category).Include(i=>i.Category.Parent)
            .Include(i => i.ProductOption).Include(i=>i.ProductsInStorage).Include(i=>i.Storage).ToListAsync();

        public override async Task<IEnumerable<Product>> GetSearch(string searchString)
        {
            return await Table
                .Where(x =>   x.Name.Contains(searchString)||( x.PartNumber != null &&  x.PartNumber.Contains(searchString))).ToListAsync();
        }

        public override async Task<IEnumerable<Product>> GetAllByIdAsync(Guid id, string name)
        {
            if (name == GroupNames.GroupBrand)
            {
                return await Table.OrderBy(o => o.Name).Where(w => w.BrandId == id)
                    .Include(i => i.Brand).Include(i => i.Category)
                    .Include(i => i.ProductOption).ToListAsync();
            }
            if(name == GroupNames.GroupCategory)
            {
               return await Table.OrderBy(o => o.Name).Where(w => w.CategoryId == id)
                    .Include(i => i.Brand).Include(i => i.Category)
                    .Include(i => i.ProductOption).ToListAsync();
            }
            return null!;
        }

        public override async Task<IEnumerable<Product>> GetAllContainsInIdsAsync(IEnumerable<Guid> ids)
        {
            return await Table.Where(w => ids.Contains(w.Id)).OrderBy(o => o.Name).ToListAsync();
        }
        //public IQueryable<Product> GetAllByQuery() =>  Table.OrderBy(o => o.Name)
        //    .Include(i => i.Brand).Include(i => i.Category)
        //    .Include(i => i.ProductOption);


        public async Task<SortPagedResponse<Product>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterProductTuple> request)
        {
            var data = Table.Include(i => i.Category!.Parent).Select(s => new Product()
            {
                Id = s.Id,
                Name = s.Name,
                Image = s.Image,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                Brand = s.Brand!,
                Category = s.Category!,
                PartNumber = s.PartNumber,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
                ProductOption = s.ProductOption,
                Price = s.Price,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
            }).Select(s => s);
        
            if (request.Chapter != null && request.ChapterId != null)
            {
                switch (request.Chapter)
                {
                    case GroupNames.GroupCategory:
                        data = data.Where(o => o.CategoryId == request.ChapterId);
                        break;
                    case GroupNames.GroupBrand:
                        data = data.Where(o => o.BrandId == request.ChapterId);
                        break;
                    //case GroupNames.:
                    //    data = data.Where(o => o.Storage!.Select(s => s.Id).Contains((Guid)request.ChapterId));
                    //    break;
                }
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                data = data.Where(w =>
                    w.Brand != null && w.Brand.Name.Contains(request.SearchString) ||
                    w.Name.Contains(request.SearchString) || w.PartNumber!.Contains(request.SearchString));
            }
            if (request.FilterTuple != null)
            {
                if ( request.FilterTuple.AllProductsToGuid != null && request.FilterTuple.AllProductsToGuid.Any())
                {
                    data = data.Where(w => request.FilterTuple.AllProductsToGuid.Contains(w.Id));
                }
                else
                {
                    if (request.FilterTuple.CategoryId != null && request.FilterTuple.CategoryId != Guid.Empty &&
                        request.Chapter != GroupNames.GroupCategory)
                    {
                        data = data.Where(o => o.CategoryId == request.FilterTuple.CategoryId);
                    }

                    if (request.FilterTuple.BrandIdList != null && request.FilterTuple.BrandIdList.Any() &&
                        request.Chapter != GroupNames.GroupBrand)
                    {
                        data = data.Where(o => request.FilterTuple.BrandIdList.Contains((Guid)o.BrandId!));
                    }

                    if (request.FilterTuple.ProductOptions != null && request.FilterTuple.ProductOptions.Any())
                    {
                        var productsId = _productOptions
                            .Where(w => request.FilterTuple.ProductOptions.Contains(w.Id))
                            .Select(o => o.ProductId).Distinct().ToList();
                        if (productsId.Any())
                            data = data.Where(w => productsId.Contains(w.Id));
                    }
                }
            }

            var totalItems = data.Count();
  
            switch (request.SortLabel)
            {
                case SortLabelsProduct.PartNumberField:
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.PartNumber);
                    break;
                case SortLabelsProduct.NameField:
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Name);
                    break;
                case SortLabelsProduct.BrandField:
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Brand!.Name);
                    break;
                case SortLabelsProduct.CreateField:
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                    break;
                case SortLabelsProduct.PriceField:
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Price);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return (new SortPagedResponse<Product>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() });

        }

        public  override async Task<bool> Update(Product entity)
        {
            Context.Entry(entity).State = EntityState.Modified;

            if (entity.ProductsInStorage != null)
            {
                var pr = await _productsInStorages.Where(w => w.ProductId == entity.Id).Select(s => s.StorageId).ToListAsync();
                foreach (var item in entity.ProductsInStorage)
                {
                    Context.Entry(item).State = pr.Contains(item.StorageId) ? EntityState.Modified : EntityState.Added;
                }

            }
            var t = await SaveChangesAsync();
            return t > 0;
        }
    }
}
