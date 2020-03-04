using System.Collections.Generic;

namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    public class Urls
    {
        /// <summary>
        /// Web page to redirect the customer on completed
        /// </summary>
        public string CompleteUrl { get; set; }

        /// <summary>
        /// Web page to redirect the customer on cancel
        /// </summary>
        public string CancelUrl { get; set; }

        /// <summary>
        /// API callback endpoint. Called on Vipps app payment sequence finished.
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// The host URLs
        /// </summary>
        public List<string> HostUrls { get; set; } = new List<string>();

        /// <summary>
        /// The URL to the terms of service
        /// </summary>
        public string TermsOfServiceUrl { get; set; }

        /// <summary>
        /// The URL to the logo
        /// </summary>
        /// <example>http://example.com/payment-logo.png</example>
        public string LogoUrl { get; set; }

        /// <summary>
        /// The URL to the perform payment page
        /// </summary>
        /// <example>http://example.com/perform-payment</example>
        public string PaymentUrl { get; set; }
    }
}