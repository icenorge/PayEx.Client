using System;
using System.Collections.Generic;

namespace PayEx.Client
{
    public class PayExOptions
    {
        public Uri ApiBaseUrl { get; set; }
        public string Token { get; set; }

        public string PayeeId { get; set; }
        public string PayeeName { get; set; }

        public Uri CallBackUrl { get; set; }
        public Uri CancelPageUrl { get; set; }
        public Uri CompletePageUrl { get; set; }
        public Uri TermsOfServiceUrl { get; set; }
        public Uri LogoUrl { get; set; }
        public Uri PaymentUrl { get; set; }
        public List<Uri> HostUrls { get; set; } = new List<Uri>();

        public bool IsEmpty()
        {
            return ApiBaseUrl == null
                   || string.IsNullOrEmpty(Token)
                   || string.IsNullOrEmpty(PayeeId);
        }

        /// <summary>
        /// This is a synonym for PayeeId. It is provided for backwards compability.
        /// </summary>
        public string MerchantId { get { return PayeeId; } set { PayeeId = value; } }

        /// <summary>
        /// This is a synonym for PayeeName. It is provided for backwards compability.
        /// </summary>      
        public string MerchantName { get { return PayeeName; } set { PayeeName = value; } }

    }
}
