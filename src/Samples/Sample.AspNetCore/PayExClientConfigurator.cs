using System;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using PayEx.Client;

namespace Sample.AspNetCore
{
    public class PayExClientConfigurator : IConfigureNamedOptions<HttpClientFactoryOptions>
    {
        private readonly PayExOptions _payexoptions;

        public PayExClientConfigurator(IOptions<PayExOptions> payexoptions)
        {
            _payexoptions = payexoptions.Value;
        }

        public void Configure(HttpClientFactoryOptions options)
        {

        }

        public void Configure(string name, HttpClientFactoryOptions options)
        {
            options.HttpClientActions.Add(client =>
            {
                client.BaseAddress = _payexoptions.ApiBaseUrl;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_payexoptions.Token}");
            });
        }
    }
}