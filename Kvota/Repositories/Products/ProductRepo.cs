using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Kvota.Constants;
using Kvota.Data;
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
            Table = context.Products;
            Context = context;
            productsInStorages = context.ProductsInStorages;

        }

        private DbSet<ProductsInStorage> productsInStorages;
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
        public IQueryable<Product> GetAllByQuery() =>  Table.OrderBy(o => o.Name)
            .Include(i => i.Brand).Include(i => i.Category)
            .Include(i => i.ProductOption);

        public override async Task<SortPagedResponse<ProductsDto>> GetBySortPagedSearchChapterAsync(SortPagedRequest request)
        {
            var data = Table.Select(s => new ProductsDto()
            {
                Id = s.Id,
                Name = s.Name,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                BrandName = s.Brand!.Name,
                CategoryName = s.Category!.Name,
                PartNumber = s.PartNumber,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
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
                    w.BrandName != null && w.BrandName.Contains(request.SearchString) ||
                    w.Name.Contains(request.SearchString) || w.PartNumber!.Contains(request.SearchString));
            }

            var totalItems = data.Count();
            var sd = request.SortDirection;
            switch (request.SortLabel)
            {
                case "category_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.CategoryName);
                    break;
                case "partNumber_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.PartNumber);
                    break;
                case "name_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Name);
                    break;
                case "brand_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.BrandName);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return (new SortPagedResponse<ProductsDto>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() });

        }

        public  override async Task<bool> Update(Product entity)
        {
            Context.Entry(entity).State = EntityState.Modified;

            if (entity.ProductsInStorage != null)
            {
                var pr = await productsInStorages.Where(w => w.ProductId == entity.Id).Select(s => s.StorageId).ToListAsync();
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
