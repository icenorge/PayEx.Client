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

        public string GetPaymentUrl() => GetRelOperationHref("redirect-authorization", "PaymentUrl");
        public string GetPaymentUrlOrDefault() => GetRelOperationHref("redirect-authorization", "PaymentUrl", false);

        public string GetRedirectVerificationUrl() => GetRelOperationHref("redirect-verification", "RedirectVerificationUrl");
        public string GetRedirectVerificationUrlOrDefault() => GetRelOperationHref("redirect-verification", "RedirectVerificationUrl", false);

        public string GetViewAuthorizationUrl() => GetRelOperationHref("view-authorization", "ViewAuthorizationUrl");
        public string GetViewAuthorizationUrlOrDefault() => GetRelOperationHref("view-authorization", "ViewAuthorizationUrl", false);

        private string GetRelOperationHref(string rel, string description, bool throwOnNull = true)
        {
            var httpOperation = Operations.FirstOrDefault(o => o.Rel == rel);
            if (throwOnNull == true 
                && httpOperation == null)
            {
                if (Operations.Any())
                {
                    var availableOps = Operations.Select(o => o.Rel).Aggregate((x, y) => x + "," + y);
                    throw new BadRequestException($"Cannot get {description} from this payment. Available operations: {availableOps}");
                }
                throw new NoOperationsLeftException();
            }
            return httpOperation?.Href;
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