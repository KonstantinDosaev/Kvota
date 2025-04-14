using Kvota.Models.Products;
using Kvota.Repositories;
using System.Net.Http;
using System.Text.Json;

namespace Kvota.RepositoriesUi.Products
{
    public class ProductRepoUi : BaseRepoUi<Product>
    {
        private readonly HttpClient _client;
        public ProductRepoUi(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            ClientFactory = clientFactory;
            _client = ClientFactory.CreateClient("api");
        }
        
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _client.GetFromJsonAsync<List<Product>>("api/product") ??
                   throw new InvalidOperationException();
        }

        public override async Task<Product> GetOneAsync(Guid id)
        {
            return await _client.GetFromJsonAsync<Product>($"api/product/{id}") ??
                   throw new InvalidOperationException();
        }

        public override async Task<IEnumerable<Product>> GetAllContainsInIdsAsync(IEnumerable<Guid> ids)
        {
            var response = await _client.PostAsJsonAsync("api/product/containsIds", ids);
            return await response.Content.ReadFromJsonAsync<List<Product>>() ?? throw new InvalidOperationException();
        }

    }
}
