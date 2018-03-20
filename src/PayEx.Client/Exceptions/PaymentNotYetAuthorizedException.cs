using System;

namespace PayEx.Client.Exceptions
{
    public class PaymentNotYetAuthorizedException : Exception
    {
        public string Id { get; }

        public PaymentNotYetAuthorizedException(string id, string message) : base(message)
        {
            Id = id;
        }
    }
}