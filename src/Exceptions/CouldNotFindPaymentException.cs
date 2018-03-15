using System;
using PayEx.Client.Models.Vipps;

namespace PayEx.Client.Exceptions
{
    public class CouldNotFindPaymentException : Exception
    {
        public ProblemsContainer Problems { get; }
        public string Id { get; }

        public CouldNotFindPaymentException(string id, ProblemsContainer problems) : base(problems.ToString())
        {
            Problems = problems;
            Id = id;
        }

        public CouldNotFindPaymentException(string id) : base("Could not find payment for the given id")
        {
            Problems = new ProblemsContainer(nameof(id), "Could not find payment for the given id");
            Id = id;
        }
    }
}