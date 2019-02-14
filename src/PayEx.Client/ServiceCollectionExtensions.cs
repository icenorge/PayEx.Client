using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PayEx.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPayExHttpClient(this IServiceCollection services, IConfiguration configuration, string clientName)
        {
            var payexConfigsection = configuration.GetSection($"PayEx:{clientName}");
            services.Configure<PayExOptions>(clientName, payexConfigsection);
            services.AddHttpClient<PayExHttpClient>(clientName);
            services.AddTransient<PayExClient>();
            return services;
        }
    }
}