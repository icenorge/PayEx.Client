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
            payment.Urls.CancelUrl = CallbackUrl(options.CancelPageUrl, "?status=cancel");
            payment.Urls.CompleteUrl = CallbackUrl(options.CompletePageUrl, "?status=complete");
        }

        private static string CallbackUrl(Uri callbackBaseUrl, string relativePath)
        {
            return new Uri(callbackBaseUrl, relativePath).ToString();
        }
    }
}