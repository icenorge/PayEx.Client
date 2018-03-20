using System;
using PayEx.Client.Models.Vipps;

namespace PayEx.Client.Exceptions
{
    public class CouldNotCancelPaymentException : Exception
    {
        public ProblemsContainer Problems { get; }
        public string Id { get; }

        public CouldNotCancelPaymentException(string id, ProblemsContainer problems) : base(problems.ToString())
        {
            Problems = problems;
            Id = id;
        }

        public CouldNotCancelPaymentException(string id, string value) : this(id, new ProblemsContainer("paymentId", value))
        {
        }
    }
}