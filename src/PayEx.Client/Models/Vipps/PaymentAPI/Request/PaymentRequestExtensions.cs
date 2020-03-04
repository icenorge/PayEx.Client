using System;
using System.Linq;

namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    internal static class PaymentRequestExtensions
    {
        internal static void SetMissingMerchantInformationFromOptions(this PaymentRequest payment, PayExOptions options)
        {
            // Make it so that the PaymentRequest can carry all the information from the call,
            // but will optionally override values from configuration if it is missing
            payment.PayeeInfo.PayeeId = payment.PayeeInfo.PayeeId ?? options.PayeeId;
            payment.PayeeInfo.PayeeName = payment.PayeeInfo.PayeeName ?? options.PayeeName;
            payment.Urls.CallbackUrl = payment.Urls.CallbackUrl ?? options.CallBackUrl.ToString();
            payment.Urls.CancelUrl = payment.Urls.CancelUrl ?? options.CancelPageUrl.ToString();
            payment.Urls.CompleteUrl = payment.Urls.CompleteUrl ?? options.CompletePageUrl.ToString();
            payment.Urls.HostUrls = payment.Urls.HostUrls ?? options.HostUrls.Select(x => x.ToString()).ToList();
            payment.Urls.PaymentUrl = payment?.Urls.PaymentUrl ?? options?.PaymentUrl?.ToString();
            payment.Urls.TermsOfServiceUrl = payment?.Urls.TermsOfServiceUrl ?? options?.TermsOfServiceUrl?.ToString();
        }
    }
}