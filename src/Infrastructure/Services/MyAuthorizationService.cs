using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Service responsible for getting and refreshing the required access_tokens.
    /// If available, think about using a distributed cache instead.
    /// </summary>
    public class MyAuthorizationService
    {
        private string _authKey => "my:cache:key";
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public MyAuthorizationService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
        }

        public async Task<AuthenticationHeaderValue> GetAuthenticationHeader()
        {
            var authentication = _cache.Get<AuthenticationHeaderValue>(_authKey);

            if (authentication == null)
                authentication = await RefreshAuthenticationHeader();

            return authentication;
        }

        public Task<AuthenticationHeaderValue> RefreshAuthenticationHeader()
        {
            var authentication = GetExternalAccessToken();
            _cache.Set(_authKey, authentication);
            return authentication;
        }


        private async Task<AuthenticationHeaderValue> GetExternalAccessToken()
        {
            var credentials = new MyServiceLogin();
            var response = await _httpClient.PostAsJsonAsync($"authorization-route", credentials);
            var authentication = JsonSerializer.Deserialize<AccessToken>(await response.Content.ReadAsStringAsync());

            return new AuthenticationHeaderValue(authentication.TokenType, authentication.Token);
        }

        #region Internal classes

        class MyServiceLogin
        {
            [JsonPropertyName("username")]
            public string Username => Environment.GetEnvironmentVariable("MY_SERVICE_USERNAME");

            [JsonPropertyName("password")]
            public string Password => Environment.GetEnvironmentVariable("MY_SERVICE_PASSWORD");
        }

        class AccessToken
        {
            [JsonPropertyName("access_token")]
            public string Token { get; set; }

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }
        }

        #endregion
    }
}
