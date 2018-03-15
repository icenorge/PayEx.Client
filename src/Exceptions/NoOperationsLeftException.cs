using System;

namespace PayEx.Client.Exceptions
{
    public class NoOperationsLeftException : Exception
    {
        public NoOperationsLeftException() : base("No available operations. Check state.")
        {
            
        }
    }
}
