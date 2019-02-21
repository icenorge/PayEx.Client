using System;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PayEx.Client;
using PayEx.Client.Models.Vipps.PaymentAPI.Common;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;
using PayEx.Client.Models.Vipps.TransactionAPI.Request;

namespace Sample.AspNetCore.Controllers
{
    public class DefaultController : Controller
    {
        private readonly PayExClient _client;

        public DefaultController(PayExClient client)
        {
            _client = client;
        }

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                InitiateARecurringPaymentNoInitialPayment = $"{Request.Scheme}://{Request.Host}{Url.Action("InitiateARecurringPaymentNoInitialPayment")}",
                InitiateARecurringPaymentIncludingInitialPayment = $"{Request.Scheme}://{Request.Host}{Url.Action("InitiateARecurringPaymentWithInitialPayment")}"
            });
        }
        
        [Route("/initiate-recurring-payment-no-initial-payment")]
        [HttpGet]
        public async Task<IActionResult> InitiateARecurringPaymentNoInitialPayment()
        {
            var paymentRequest = new PaymentRequest("Sample.AspNetCore/1.0.0", "Some productname", "order123", GenerateReference(), null);
            paymentRequest.GeneratePaymentToken = true;
            paymentRequest.Operation = "Verify";
            
            var res = await _client.PostCreditCardPayment(paymentRequest);
            
            return Ok(new 
            {
                PaymentDetails =  $"{Request.Scheme}://{Request.Host}{Url.Action("PaymentDetails", new { PaymentId = res.Payment.Id})}",
                UrlForCustomerToVerifyRecurringPayment = res.GetRedirectVerificationUrl(),
            });
        }
        
        [Route("/initiate-recurring-payment-with-initial-payment")]
        [HttpGet]
        public async Task<IActionResult> InitiateARecurringPaymentWithInitialPayment()
        {
            var intialPaymentToPayAlongWithSettingUpRecurringPayments = new Price
            {
                Amount = 20000,
                VatAmount = 5000,
                Type = PriceTypes.Visa
            };

            var paymentRequest = new PaymentRequest("Sample.AspNetCore/1.0.0", "Some productname", "order123", GenerateReference(), intialPaymentToPayAlongWithSettingUpRecurringPayments);
            paymentRequest.GeneratePaymentToken = true;
            paymentRequest.Operation = "Purchase";
            
            var res = await _client.PostCreditCardPayment(paymentRequest);
            
            return Ok(new 
            {
                PaymentDetails =  $"{Request.Scheme}://{Request.Host}{Url.Action("PaymentDetails", new { PaymentId = res.Payment.Id})}",
                UrlForCustomerToVerifyRecurringPayment = res.GetPaymentUrl(),
            });
        }

        [Route("/paymentdetails")]
        [HttpGet]
        public async Task<IActionResult> PaymentDetails(string paymentId)
        {
            var res = await _client.GetPayment(paymentId);
            
            var manualCapture = Url.Action("CreateAndCaptureManually", new
            {
                PaymentToken = res.Payment.PaymentToken
            });
            
            var autocapture = Url.Action("CreateAndCaptureAutomatically", new
            {
                PaymentToken = res.Payment.PaymentToken
            });
            
            return Ok(new 
            {
                CreateNewPaymentUsingManualCapture =  $"{Request.Scheme}://{Request.Host}{manualCapture}",
                CreateNewPaymentUsingAutoCapture = $"{Request.Scheme}://{Request.Host}{autocapture}"
            });
        }

        [Route("/capture-manually")]
        [HttpGet]
        public async Task<IActionResult> CreateAndCaptureManually(string paymentToken)
        {
      
            var paymentRequest = new PaymentRequest("Sample.AspNetCore/1.0.0", "Some productname", "order123", GenerateReference());
            paymentRequest.Operation = "Recur";
            paymentRequest.PaymentToken = paymentToken;
            paymentRequest.Amount = 10000;
            paymentRequest.VatAmount = 2500;
            paymentRequest.Description = "Some recurring stuff";
            
            var res = await _client.PostCreditCardPayment(paymentRequest);


            await _client.PostCapture(res.Payment.Id, new TransactionRequest
            {
                Amount = 10000,
                VatAmount = 2500,
                Description = "CApturE of the recurring stuff!",
                PayeeReference = GenerateReference()
            });
            
            var values = new
            {
                PaymentId = res.Payment.Id,
                PaymentToken = res.Payment.PaymentToken
            };
            return Ok(new 
            {
                PaymentDetails = $"{Request.Scheme}://{Request.Host}{Url.Action("paymentdetails", new { paymentId = res.Payment.Id })}",
                LinkToAutoCaptureAnotherPayment =  $"{Request.Scheme}://{Request.Host}{Url.Action("CreateAndCaptureAutomatically", new { paymentToken })}",
                NextPurchase = $"{Request.Scheme}://{Request.Host}{Url.Action("CreateAndCaptureManually", values)}",
                Payment = res.Payment
            });
        }
        
        [Route("/auto-capture")]
        [HttpGet]
        public async Task<IActionResult> CreateAndCaptureAutomatically(string paymentToken)
        {
            var paymentRequest = new PaymentRequest("Sample.AspNetCore/1.0.0", "Some autocaptured productname", "auto-capture-order123", GenerateReference());
            paymentRequest.Operation = "Recur";
            paymentRequest.Intent = "AutoCapture";
            paymentRequest.PaymentToken = paymentToken;
            paymentRequest.Amount = 50000;
            paymentRequest.VatAmount = 7700;
            paymentRequest.Description = "Some auto-captured stuff";
            
            var res = await _client.PostCreditCardPayment(paymentRequest);

            var values = new
            {
                PaymentId = res.Payment.Id,
                PaymentToken = res.Payment.PaymentToken
            };
            return Ok(new 
            {
                CreateAndCaptureAnotherPayment =  $"{Request.Scheme}://{Request.Host}{Url.Action("CreateAndCaptureAutomatically", new { paymentToken })}",
                NextPurchase = $"{Request.Scheme}://{Request.Host}{Url.Action("CreateAndCaptureManually", values)}",
                Payment = res.Payment
            });
        }

        private static string GenerateReference()
        {
            return new Random().Next(1337,133713371).ToString();
        }
    }
}