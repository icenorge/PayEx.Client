using System;

namespace PayEx.Client
{
    public class PayExOptions
    {
        public Uri ApiBaseUrl { get; set; }
        public string Token { get; set; }
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public Uri CallBackUrl { get; set; }
        public Uri CancelPageUrl { get; set; }
        public Uri CompletePageUrl { get; set; }
    }
}
