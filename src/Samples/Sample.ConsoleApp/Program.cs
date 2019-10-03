using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
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
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var payExOptions = new PayExOptions();
            config.GetSection("payex:someAccount").Bind(payExOptions);

            IOptions<PayExOptions> options = new OptionsWrapper<PayExOptions>(payExOptions);

            IHttpClientFactory httpClientFactory = new HttpClientCreator(options.Value);

            ISelectClient clientSelector = new DummySelector();


            IConfigureOptions<PayExOptions> optionsConfigurator = new ConfigureOptions<PayExOptions>(Conf(payExOptions));
            var configureOptionses = new []{ optionsConfigurator };
            IPostConfigureOptions<PayExOptions> postoptionsConfigurator = new PostConfigureOptions<PayExOptions>(Constants.THECLIENTNAME,Conf(payExOptions));
            var postConfigureOptionses = new []{ postoptionsConfigurator};
            
            IOptionsSnapshot<PayExOptions> optionsSnap = new OptionsManager<PayExOptions>(new OptionsFactory<PayExOptions>(configureOptionses,postConfigureOptionses));
            var dynamic = new PayExClientDynamic(httpClientFactory, optionsSnap);
            var payExClient = new PayExClient(dynamic, clientSelector);
            var prices = new Price
            {
                Amount = 10000,
                VatAmount = 2500,
                Type = PriceTypes.Vipps
            };
            var paymentRequest = new PaymentRequest("Console.Sample/1.0.0", "Some productname", "order123", "123456", prices);
            var res = payExClient.PostVippsPayment(paymentRequest).GetAwaiter().GetResult();
            Console.WriteLine($"Payment created with id : {res.Payment.Id}");

            var dynamicallyCreated = dynamic.PostVippsPayment(clientSelector.Select(), paymentRequest).GetAwaiter().GetResult();
            Console.WriteLine($"Payment created with dynamic client. id : {dynamicallyCreated.Payment.Id}");

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
        public const string THECLIENTNAME = "someAccount";
    }
}
