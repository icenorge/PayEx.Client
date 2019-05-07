using System;

namespace PayEx.Client
{
    public class PayExOptions
    {
        public PayExOptions()
        {
            this.StatusInUrl = true;
        }

        public Uri ApiBaseUrl { get; set; }
        public string Token { get; set; }
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public Uri CallBackUrl { get; set; }
        public Uri CancelPageUrl { get; set; }
        public Uri CompletePageUrl { get; set; }

        public bool StatusInUrl { get; set; }

        public bool IsEmpty()
        {
            return ApiBaseUrl == null || string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(MerchantId);
        }
    }
}
