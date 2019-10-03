using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;
using PayEx.Client.Models.Vipps.PaymentAPI.Response;
using PayEx.Client.Models.Vipps.TransactionAPI.Request;
using PayEx.Client.Models.Vipps.TransactionAPI.Response;

namespace PayEx.Client
{
    public class PayExClient
    {
        private readonly PayExClientDynamic _dynamic;
        private readonly ISelectClient _clientSelector;

        public PayExClient(PayExClientDynamic dynamic, ISelectClient clientSelector)
        {
            _dynamic = dynamic;
            _clientSelector = clientSelector;
            var selector = clientSelector.Select();
            if (string.IsNullOrEmpty(selector))
                throw new Exception("No clientname given. Check ISelectClient and/or any configuration.");
        }
        
        /// <summary>
        /// Gets an existing payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PaymentResponseContainer> GetPayment(string id)
        {
            return _dynamic.GetPayment(_clientSelector.Select(), id);
        }
        
        /// <summary>
        /// Creates a new Vipps payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public Task<PaymentResponseContainer> PostVippsPayment(PaymentRequest payment)
        {
            return _dynamic.PostVippsPayment(_clientSelector.Select(), payment);
        }

        /// <summary>
        /// Creates a new CreditCard payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public Task<PaymentResponseContainer> PostCreditCardPayment(PaymentRequest payment)
        {
            return _dynamic.PostCreditCardPayment(_clientSelector.Select(), payment);
        }

        /// <summary>
        /// Gets all transactions for a payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<TransactionResponse>> GetTransactions(string id)
        {
            return _dynamic.GetTransactions(_clientSelector.Select(), id);
        }

        /// <summary>
        /// Captures a payment a.k.a POSTs a transaction.
        /// </summary>
        public Task<TransactionResponse> PostCapture(string id, TransactionRequest transaction)
        {
            return _dynamic.PostCapture(_clientSelector.Select(), id, transaction);
        }

        /// <summary>
        /// Reverses a payment a.k.a POSTs a transaction.
        /// </summary>
        public Task<TransactionResponse> PostReversal(string id, TransactionRequest transaction)
        {
            return _dynamic.PostReversal(_clientSelector.Select(), id, transaction);
        }

        /// <summary>
        /// Cancels a payment a.k.a POSTs a transaction.
        /// </summary>
        public Task<TransactionResponse> PostCancellation(string id, TransactionRequest transaction)
        {
            return _dynamic.PostCancellation(_clientSelector.Select(), id, transaction);
        }

        /// <summary>
        /// Cancels an unauthorized creditcard payment a.k.a PATCH with a static abort object.
        /// </summary>
        public Task<PaymentResponseContainer> PatchAbortPayment(string id)
        {
            return _dynamic.PatchAbortPayment(_clientSelector.Select(), id);
        }

        public Task<string> GetRaw(string id)
        {
            return _dynamic.GetRaw(_clientSelector.Select(), id);
        }
    }

    public class UnknownAccountException : Exception
    {
        public UnknownAccountException(string message) : base(message)
        {
            
        }
    }
}
