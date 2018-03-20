using System;
using PayEx.Client.Models.Vipps;

namespace PayEx.Client.Exceptions
{
    public class CouldNotReversePaymentException : Exception
    {
        public ProblemsContainer Problems { get; }
        public string Id { get; }

        public CouldNotReversePaymentException(string id, ProblemsContainer problems) : base(problems.ToString())
        {
            Problems = problems;
            Id = id;
        }

        public CouldNotReversePaymentException(string id, string value) : this(id, new ProblemsContainer("paymentId", value))
        {
        }
    }
}