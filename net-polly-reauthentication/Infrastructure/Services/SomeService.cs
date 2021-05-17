using Domain.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Any service that makes http calls. In our case, it also requires an access_token
    /// that we set as we inject the HttpClient. See Crosscutting.DependencyResolver
    /// for more details.
    /// </summary>
    public class SomeService : ISomeService
    {
        private readonly HttpClient _httpClient;

        public SomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetSomething()
        {
            var response = await _httpClient.GetFromJsonAsync<List<string>>($"my-resource");
            return response;
        }
    }
}
