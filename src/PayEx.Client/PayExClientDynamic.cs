using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PayEx.Client.Exceptions;
using PayEx.Client.Models.Vipps;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;
using PayEx.Client.Models.Vipps.PaymentAPI.Response;
using PayEx.Client.Models.Vipps.TransactionAPI.Request;
using PayEx.Client.Models.Vipps.TransactionAPI.Response;

namespace PayEx.Client
{
    public class PayExClientDynamic
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string PspVippsPaymentsBaseUrl = "/psp/vipps/payments/";
        private const string PspCreditCardPaymentsBaseUrl = "/psp/creditcard/payments/";
        private readonly ILogPayExHttpResponse _logger;
        private readonly IOptionsSnapshot<PayExOptions> _optionFetcher;

        public PayExClientDynamic(IHttpClientFactory clientFactory, IOptionsSnapshot<PayExOptions> options, ILogPayExHttpResponse logger = null)
        {
            _clientFactory = clientFactory;
            _logger = logger ?? new NoOpLogger();
            _optionFetcher = options;
        }

        /// <summary>
        /// Creates a new Vipps payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public async Task<PaymentResponseContainer> PostVippsPayment(string clientName, PaymentRequest payment)
        {
            return await CreatePayment(PspVippsPaymentsBaseUrl, clientName, payment);
        }

        /// <summary>
        /// Creates a new CreditCard payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public async Task<PaymentResponseContainer> PostCreditCardPayment(string clientName,PaymentRequest payment)
        {
            return await CreatePayment(PspCreditCardPaymentsBaseUrl, clientName, payment);
        }

        /// <summary>
        /// Gets an existing payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PaymentResponseContainer> GetPayment(string clientName, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new CouldNotFindPaymentException(id);

            var url = $"{id}?$expand=prices,captures,payeeinfo,urls,transactions,authorizations,reversals,cancellations";
            Func<ProblemsContainer, Exception> onError = m => new CouldNotFindPaymentException(id, m);
            var res = await CreateInternalClient(clientName).HttpGet<PaymentResponseContainer>(url, onError);
            return res;
        }

        /// <summary>
        /// Gets all transactions for a payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TransactionResponse>> GetTransactions(string clientName, string id)
        {
            var url = $"{id}?$expand=prices,captures,payeeinfo,urls,transactions,authorizations,reversals,cancellations";
            Func<ProblemsContainer, Exception> onError = m => new CouldNotFindTransactionException(id, m);
            var res = await CreateInternalClient(clientName).HttpGet<AllTransactionResponseContainer>(url, onError);
            return res.Transactions.TransactionList;
        }

        /// <summary>
        /// Captures a payment a.k.a POSTs a transaction.
        /// </summary>
        public async Task<TransactionResponse> PostCapture(string clientName, string id, TransactionRequest transaction)
        {
            var payment = await GetPayment(clientName,id);

            var httpOperation = payment.Operations.FirstOrDefault(o => o.Rel == "create-capture");
            if (httpOperation == null)
            {
                if (payment.Operations.Any())
                {
                    var availableOps = payment.Operations.Select(o => o.Rel).Aggregate((x, y) => x + "," + y);
                    throw new PaymentNotYetAuthorizedException(id, $"This payment cannot be captured. Available operations: {availableOps}");
                }
                throw new NoOperationsLeftException();
            }

            var url = httpOperation.Href;
            Func<ProblemsContainer, Exception> onError = m => new CouldNotPostTransactionException(id, m);
            var payload = new TransactionRequestContainer(transaction);
            var res = await CreateInternalClient(clientName).HttpPost<TransactionRequestContainer, CaptureTransactionResponseContainer>(url, onError, payload);
            return res.Capture.Transaction;
        }

        /// <summary>
        /// Reverses a payment a.k.a POSTs a transaction.
        /// </summary>
        public async Task<TransactionResponse> PostReversal(string clientName, string id, TransactionRequest transaction)
        {
            var payment = await GetPayment(clientName,id);

            var httpOperation = payment.Operations.FirstOrDefault(o => o.Rel == "create-reversal");
            if (httpOperation == null)
            {
                if (payment.Operations.Any())
                {
                    var availableOps = payment.Operations.Select(o => o.Rel).Aggregate((x, y) => x + "," + y);
                    throw new CouldNotReversePaymentException(id, $"This payment cannot be reversed. Available operations: {availableOps}");
                }
                throw new NoOperationsLeftException();
            }

            var url = httpOperation.Href;
            Func<ProblemsContainer, Exception> onError = m => new CouldNotPostTransactionException(id, m);
            var payload = new TransactionRequestContainer(transaction);
            var res = await CreateInternalClient(clientName).HttpPost<TransactionRequestContainer, ReversalTransactionResponseContainer>(url, onError, payload);
            return res.Reversal.Transaction;
        }

        /// <summary>
        /// Cancels a payment a.k.a POSTs a transaction.
        /// </summary>
        public async Task<TransactionResponse> PostCancellation(string clientName,string id, TransactionRequest transaction)
        {
            var payment = await GetPayment(clientName,id);

            var httpOperation = payment.Operations.FirstOrDefault(o => o.Rel == "create-cancellation");
            if (httpOperation == null)
            {
                if (payment.Operations.Any())
                {
                    var availableOps = payment.Operations.Select(o => o.Rel).Aggregate((x, y) => x + "," + y);
                    throw new CouldNotCancelPaymentException(id, $"This payment cannot be cancelled. Available operations: {availableOps}");
                }
                throw new NoOperationsLeftException();
            }

            var url = httpOperation.Href;
            Func<ProblemsContainer, Exception> onError = m => new CouldNotPostTransactionException(id, m);
            var payload = new TransactionRequestContainer(transaction);
            var res = await CreateInternalClient(clientName).HttpPost<TransactionRequestContainer, CancellationTransactionResponseContainer>(url, onError, payload);
            return res.Cancellation.Transaction;
        }

        /// <summary>
        /// Cancels an unauthorized creditcard payment a.k.a PATCH with a static abort object.
        /// </summary>
        public async Task<PaymentResponseContainer> PatchAbortPayment(string clientName, string id)
        {
            var payment = await GetPayment(clientName, id);

            var httpOperation = payment.Operations.FirstOrDefault(o => o.Rel == "update-payment-abort");
            if (httpOperation == null)
            {
                if (payment.Operations.Any())
                {
                    var availableOps = payment.Operations.Select(o => o.Rel).Aggregate((x, y) => x + "," + y);
                    throw new CouldNotCancelPaymentException(id, $"This payment cannot be aborted. Available operations: {availableOps}");
                }
                throw new NoOperationsLeftException();
            }

            var url = httpOperation.Href;
            Func<ProblemsContainer, Exception> onError = m => new CouldNotPostTransactionException(id, m);
            var payload = new PaymentAbortRequestContainer();
            var res = await CreateInternalClient(clientName).HttpPatch<PaymentAbortRequestContainer, PaymentResponseContainer>(url, onError, payload);
            return res;
        }

        public Task<string> GetRaw(string clientName, string id)
        {
            var url = $"{id}?$expand=prices,captures,payeeinfo,urls,transactions,authorizations,reversals,cancellations";
            return CreateInternalClient(clientName).GetRaw(url);
        }

        private async Task<PaymentResponseContainer> CreatePayment(string baseUrl, string clientName, PaymentRequest payment)
        {
            payment.SetMissingMerchantInformationFromOptions(Options(clientName));

            var payload = new PaymentRequestContainer(payment);
            Func<ProblemsContainer, Exception> onError = m => new CouldNotPlacePaymentException(payment, m);
            var url = $"{baseUrl}?$expand=prices,captures,payeeinfo,urls,transactions,authorizations,reversals,cancellations";
            var res = await CreateInternalClient(clientName).HttpPost<PaymentRequestContainer, PaymentResponseContainer>(url, onError, payload);
            return res;
        }

        private PayExHttpClient CreateInternalClient(string clientName)
        {
            var httpClient = _clientFactory.CreateClient(clientName);
            return new PayExHttpClient(httpClient, _logger);
        }

        private PayExOptions Options(string clientName)
        {
            var payExOptions = _optionFetcher.Get(clientName);
            
            if(payExOptions == null)
                throw new UnknownAccountException($"Unknown payex account {clientName}. Check config.");

            if (payExOptions.IsEmpty())
            {
                throw new UnknownAccountException($"Unknown payex account {clientName}. Check config.");
            }

            return payExOptions;
        }
    }
}