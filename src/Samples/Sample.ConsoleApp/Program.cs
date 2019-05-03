using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using PayEx.Client;
using PayEx.Client.Models.Vipps.PaymentAPI.Common;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;

namespace Sample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var payExOptions = new PayExOptions
            {
                ApiBaseUrl = new Uri("https://api.externalintegration.payex.com/"),
                Token = "my-token",
                MerchantId = "my-merchantId",
                MerchantName = "YOUR-MERCHANT-NAME",
                CallBackUrl = new Uri("https://yourdomain.com/callbacks"),
                CancelPageUrl = new Uri("https://yourdomain.com/cancel"),
                CompletePageUrl = new Uri("https://yourdomain.com/complete"),
                AppendStatus = true
            };

            IOptions<PayExOptions> options = new OptionsWrapper<PayExOptions>(payExOptions);

            IHttpClientFactory httpClientFactory = new HttpClientCreator(options.Value);

            ISelectClient clientSelector = new DummySelector();


            IConfigureOptions<PayExOptions> optionsConfigurator = new ConfigureOptions<PayExOptions>(Conf(payExOptions));
            var configureOptionses = new[] { optionsConfigurator };
            IPostConfigureOptions<PayExOptions> postoptionsConfigurator = new PostConfigureOptions<PayExOptions>(Constants.THECLIENTNAME, Conf(payExOptions));
            var postConfigureOptionses = new[] { postoptionsConfigurator };

            IOptionsSnapshot<PayExOptions> optionsSnap = new OptionsManager<PayExOptions>(new OptionsFactory<PayExOptions>(configureOptionses, postConfigureOptionses));
            var payExClient = new PayExClient(httpClientFactory, optionsSnap, clientSelector);
            var prices = new Price
            {
                Amount = 10000,
                VatAmount = 2500,
                Type = PriceTypes.Vipps
            };
            var paymentRequest = new PaymentRequest("Console.Sample/1.0.0", "Some productname", "order123", "123456", prices);
            var res = payExClient.PostVippsPayment(paymentRequest).GetAwaiter().GetResult();
            Console.WriteLine($"Payment created with id : {res.Payment.Id}");
        }

        private static Action<PayExOptions> Conf(PayExOptions payExOptions)
        {
            return o =>
            {
                o.ApiBaseUrl = payExOptions.ApiBaseUrl;
                o.Token = payExOptions.Token;
                o.MerchantId = payExOptions.MerchantId;
                o.MerchantName = payExOptions.MerchantName;
                o.CallBackUrl = payExOptions.CallBackUrl;
                o.CancelPageUrl = payExOptions.CancelPageUrl;
                o.CompletePageUrl = payExOptions.CompletePageUrl;
                o.ApiBaseUrl = payExOptions.ApiBaseUrl;
            };
        }
    }

    internal class DummySelector : ISelectClient
    {
        public string Select()
        {
            return Constants.THECLIENTNAME;
        }
    }

    internal class HttpClientCreator : IHttpClientFactory
    {
        private readonly PayExOptions _payExOptions;

        public HttpClientCreator(PayExOptions payExOptions)
        {
            _payExOptions = payExOptions;
        }
        public HttpClient CreateClient(string name)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_payExOptions.Token}");
            httpClient.BaseAddress = _payExOptions.ApiBaseUrl;
            return httpClient;
        }
    }

    public static class Constants
    {
        public const string THECLIENTNAME = "something";
    }
}
