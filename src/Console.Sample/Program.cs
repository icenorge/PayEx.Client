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
                Token = "YOUR-PAYEX-ACCESS-TOKEN",
                MerchantId = "YOUR-MERCHANT-ID",
                MerchantName = "YOUR-MERCHANT-NAME",
                CallBackUrl = new Uri("https://yourdomain.com/callbacks"),
                CancelPageUrl = new Uri("https://yourdomain.com/cancel"),
                CompletePageUrl = new Uri("https://yourdomain.com/complete")
            };

            IOptions<PayExOptions> options = new OptionsWrapper<PayExOptions>(payExOptions);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {payExOptions.Token}");

            var payExClient = new PayExClient(httpClient, options);
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
    }
}
