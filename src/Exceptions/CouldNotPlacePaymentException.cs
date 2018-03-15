using System;
using System.Linq;
using PayEx.Client.Models.Vipps;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;

namespace PayEx.Client.Exceptions
{
    public class CouldNotPlacePaymentException : Exception
    {
        public ProblemsContainer Problems { get; }
        public PaymentRequest Payment { get; }

        public CouldNotPlacePaymentException(PaymentRequest payment, ProblemsContainer problems) : base(problems.ToString())
        {
            Problems = problems;
            Payment = payment;
        }
    }
}