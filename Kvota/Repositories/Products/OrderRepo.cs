using Kvota.Constants;
using Kvota.Migrations;
using Kvota.Models;
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;


using MudBlazor;

namespace Kvota.Repositories.Products
{
    public class OrderRepo:BaseRepo<ApplicationOrderingProducts>
    {
        public OrderRepo(KvotaProductContext context) : base(context)
        {
            Table = context.ApplicationOrderingProducts;
        }

        public override Task<ApplicationOrderingProducts> AddAsync(ApplicationOrderingProducts entity)
        {
            var count = Table.Count(w => w.DateTimeCreated!.Value.Date == entity.DateTimeCreated!.Value.Date);
            entity.Number = (entity.DateTimeCreated!.Value.ToShortDateString()).Replace(".","") + (count + 1);
            return base.AddAsync(entity);
        }
        
        public async Task<SortPagedResponse<ApplicationOrderingProducts>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request)
        {
            var data = Table.Select(s => new ApplicationOrderingProducts()
            {
                Id = s.Id,
                Number = s.Number,
                CompanyName = s.CompanyName,
                CompanyInn = s.CompanyInn,
                ProductList = s.ProductList, 
                UserName = s.UserName, 
                DateTimeCreated = s.DateTimeCreated, 
                DateTimeUpdate = s.DateTimeUpdate, 
                ApplicationOrderingProductProduct = s.ApplicationOrderingProductProduct,
                InWork = s.InWork,
                IsDeleted = s.IsDeleted,
                Email = s.Email, 
                UpdatedUser = s.UpdatedUser, 
                MissingProductsInCatalog = s.MissingProductsInCatalog, 
                Phone = s.Phone, 
                UserId = s.UserId
            }).Select(s => s);
            //if (request.Chapter != null && request.ChapterId != null)
            //{
            //    switch (request.Chapter)
            //    {
            //        case GroupNames.GroupCategory:
            //            data = data.Where(o => o.CategoryId == request.ChapterId);
            //            break;
            //        case GroupNames.GroupBrand:
            //            data = data.Where(o => o.BrandId == request.ChapterId);
            //            break;
            //            //case GroupNames.:
            //            //    data = data.Where(o => o.Storage!.Select(s => s.Id).Contains((Guid)request.ChapterId));
            //            //    break;
            //    }
            //}
            if (request.FilterTuple is { UserId: { } })
            {
                data = data.Where(w => w.UserId == request.FilterTuple.UserId);
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                data = data.Where(w =>
                    w.Number.Contains(request.SearchString)
                    || w.CompanyName!.ToLower().Contains(request.SearchString.ToLower()));
                if (request.FilterTuple != null)
                {
                    request.FilterTuple.CreateDateFirst = null;
                    request.FilterTuple.CreateDateLast = null;
                    request.FilterTuple.UpdateDateFirst = null;
                    request.FilterTuple.UpdateDateLast = null;
                }
            }
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.InWork != null)
                {
                    data = request.FilterTuple.InWork==true ? data.Where(w => w.InWork == true) : data.Where(w => w.InWork == false);
                }
                
                if (request.FilterTuple.CreateDateFirst != null && request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeCreated!.Value.Date >= request.FilterTuple.CreateDateFirst.Value.Date
                                           && w.DateTimeCreated.Value.Date <= request.FilterTuple.CreateDateLast.Value.Date);
                }
                if (request.FilterTuple.UpdateDateFirst != null && request.FilterTuple.UpdateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeUpdate!.Value.Date >= request.FilterTuple.UpdateDateFirst.Value.Date
                                           && w.DateTimeUpdate.Value.Date <= request.FilterTuple.UpdateDateLast.Value.Date);
                }
            }
            
            var totalItems = data.Count();
            var sd = request.SortDirection;
            switch (request.SortLabel)
            {
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!,o => o.DateTimeCreated);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
            }

            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return (new SortPagedResponse<ApplicationOrderingProducts>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() });

        }
    }
}
