using System;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PayEx.Client;
using PayEx.Client.Models.Vipps.PaymentAPI.Common;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;

namespace Sample.AspNetCore.Controllers
{
    public class DefaultController : Controller
    {
        private readonly PayExClient _client;

        public DefaultController(IHttpClientFactory factory, IServiceProvider serviceProvider)
        {
            var httpClient = factory.CreateClient(Constants.Someaccount);
            _client = serviceProvider.GetService<PayExClient>();
        }

        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var prices = new Price
            {
                Amount = 10000,
                VatAmount = 2500,
                Type = PriceTypes.Vipps
            };

            var paymentRequest = new PaymentRequest("Sample.AspNetCore/1.0.0", "Some productname", "order123", "123456", prices);
            var res = await _client.PostVippsPayment(paymentRequest);
            return Ok($"Payment created with id : {res.Payment.Id}");
        }
    }
}