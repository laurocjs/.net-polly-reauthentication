using Crosscutting;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static void AddDependencyResolver(this IServiceCollection services)
        {
            RegisterAuthorizations(services);
            RegisterServices(services);
            RegisterCache(services);
        }

        private static void RegisterAuthorizations(IServiceCollection services)
        {
            services.AddHttpClient<MyAuthorizationService>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient<ISomeService, SomeService>(HttpConfigs.SomeService());
        }

        private static void RegisterCache(IServiceCollection services)
        {
            services.AddMemoryCache();
        }
    }
}
