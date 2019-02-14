using System;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using PayEx.Client;

namespace Sample.AspNetCore
{
    public class PayExClientConfigurator : IConfigureNamedOptions<HttpClientFactoryOptions>
    {
        private readonly IOptionsSnapshot<PayExOptions> _payexoptionsSnapshot;

        public PayExClientConfigurator(IOptionsSnapshot<PayExOptions> payexoptions)
        {
            _payexoptionsSnapshot = payexoptions;
        }

        public void Configure(HttpClientFactoryOptions options)
        {
        }

        public void Configure(string name, HttpClientFactoryOptions options)
        {
            var payexoptions = _payexoptionsSnapshot.Get(name);
            
            options.HttpClientActions.Add(client =>
            {
                client.BaseAddress = payexoptions.ApiBaseUrl;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {payexoptions.Token}");
                
            }); 
        }
    }
}