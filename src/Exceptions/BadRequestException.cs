using System;

namespace PayEx.Client.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(Exception e) : base(e.Message, e)
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }
    }
}
