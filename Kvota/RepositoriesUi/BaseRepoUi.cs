using Kvota.Interfaces;
using System.Text.Json;

namespace Kvota.RepositoriesUi
{
    public class BaseRepoUi<T> : IRepo<T> where T : class, IIdentifiable, new()
    {
        public  IHttpClientFactory ClientFactory;
        public BaseRepoUi(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        //public async Task<List<HolidayResponseModel>> GetHolidays(HolidayRequestModel holidaysRequest)
        //{
        //    var result = new List<HolidayResponseModel>();

        //    var url = string.Format("https://date.nager.at/api/v2/PublicHolidays/{0}/{1}",
        //        holidaysRequest.Year, holidaysRequest.CountryCode);

        //    var request = new HttpRequestMessage(HttpMethod.Get, url);
        //    request.Headers.Add("Accept", "application/vnd.github.v3+json");

        //    var client = _clientFactory.CreateClient();

        //    var response = await client.SendAsync(request);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var stringResponse = await response.Content.ReadAsStringAsync();

        //        result = JsonSerializer.Deserialize<List<HolidayResponseModel>>(stringResponse,
        //            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        //    }
        //    else
        //    {
        //        result = Array.Empty<HolidayResponseModel>().ToList();
        //    }

        //    return result;
        //}
        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetOneAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetAllContainsInIdsAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllByIdAsync(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task ManualSaveAsync()
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public int Save(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetSearch(string searchString)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllByQuery()
        {
            throw new NotImplementedException();
        }
    }
}
