using System;

namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    internal static class PaymentRequestExtensions
    {
        internal static void SetRequiredMerchantInfo(this PaymentRequest payment, PayExOptions options)
        {
            payment.PayeeInfo.PayeeId = options.MerchantId;
            payment.PayeeInfo.PayeeName = options.MerchantName;
            payment.Urls.CallbackUrl = options.CallBackUrl?.ToString();
            payment.Urls.CancelUrl = options.StatusInUrl ? CallbackUrl(options.CancelPageUrl, "?status=cancel") : options.CancelPageUrl.ToString();
            payment.Urls.CompleteUrl = options.StatusInUrl ? CallbackUrl(options.CompletePageUrl, "?status=complete") : options.CompletePageUrl.ToString();
        }

        private static string CallbackUrl(Uri callbackBaseUrl, string relativePath)
        {
            return new Uri(callbackBaseUrl, relativePath).ToString();
        }
    }
}