using System;
using System.Collections.Generic;
using System.Linq;
using PayEx.Client.Exceptions;

namespace PayEx.Client.Models.Vipps.PaymentAPI.Response
{
    public class PaymentResponseContainer
    {
        public PaymentResponseContainer(PaymentResponse payment)
        {
            Payment = payment;
        }
        public PaymentResponse Payment { get; set; }

        public List<HttpOperation> Operations { get; set; } = new List<HttpOperation>();

        public string GetPaymentUrl()
        {
            var httpOperation = Operations.FirstOrDefault(o => o.Rel == "redirect-authorization");
            if (httpOperation == null)
            {
                if (Operations.Any())
                {
                    var availableOps = Operations.Select(o => o.Rel).Aggregate((x, y) => x + "," + y);
                    throw new BadRequestException($"Cannot get PaymentUrl from this payment. Available operations: {availableOps}");
                }
                throw new NoOperationsLeftException();
            }
            return httpOperation.Href;
        }

        public string TryGetPaymentUrl()
        {
            try
            {
                return GetPaymentUrl();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}