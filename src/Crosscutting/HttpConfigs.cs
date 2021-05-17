using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Crosscutting
{
    [ExcludeFromCodeCoverage]
    public static class HttpConfigs
    {
        private static string MyServiceUri => Environment.GetEnvironmentVariable("MY_SERVICE_URI");
        private static string MyServiceClientId => Environment.GetEnvironmentVariable("MY_SERVICE_CLIENT_ID");

        public static Action<IServiceProvider, HttpClient> SomeService()
        {
            return (services, client) =>
            {
                client.BaseAddress = new Uri(MyServiceUri);
                client.DefaultRequestHeaders.Add("client_id", MyServiceClientId);

                var authenticationHeader = services.GetRequiredService<MyAuthorizationService>().GetAuthenticationHeader();
                client.DefaultRequestHeaders.Authorization = authenticationHeader.GetAwaiter().GetResult();
            };
        }
    }
}
