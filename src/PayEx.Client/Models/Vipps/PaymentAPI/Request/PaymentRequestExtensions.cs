using System;

namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    internal static class PaymentRequestExtensions
    {
        internal static void SetRequiredMerchantInfo(this PaymentRequest payment, PayExOptions options)
        {
            payment.PayeeInfo.PayeeId = options.MerchantId;
            payment.PayeeInfo.PayeeName = options.MerchantName;
            payment.Urls.CallbackUrl = CallbackUrl(options, options.CallBackUrl, string.Empty);
            payment.Urls.CancelUrl = CallbackUrl(options, options.CancelPageUrl, "?status=cancel");
            payment.Urls.CompleteUrl = CallbackUrl(options, options.CompletePageUrl, "?status=complete");
        }

        private static string CallbackUrl(PayExOptions options, Uri callbackBaseUrl, string relativePath)
        {
            if (callbackBaseUrl == null)
                return null;

            if (options.AppendStatus == false)
                return new Uri(callbackBaseUrl, string.Empty).ToString();

            return new Uri(callbackBaseUrl, relativePath).ToString();
        }
    }
}