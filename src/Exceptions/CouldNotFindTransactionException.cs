using System;
using PayEx.Client.Models.Vipps;

namespace PayEx.Client.Exceptions
{
    public class CouldNotFindTransactionException : Exception
    {
        public ProblemsContainer Problems { get; }
        public string Id { get; }

        public CouldNotFindTransactionException(string id, ProblemsContainer problems) : base(problems.ToString())
        {
            Problems = problems;
            Id = id;
        }

        public CouldNotFindTransactionException(string id) : base("Could not find transaction for the given id")
        {
            Problems = new ProblemsContainer(nameof(id), "Could not find transaction for the given id");
            Id = id;
        }

        public CouldNotFindTransactionException(string id, string name, string desc) : this(id, new ProblemsContainer(name, desc))
        {
            
        }

        public CouldNotFindTransactionException(Exception inner) : base("Could not find transaction for the given id", inner)
        {
            Problems = new ProblemsContainer("Other", "Could not find transaction for the given id");
        }
    }
}