using System;

namespace PayEx.Client.Exceptions
{
    public class InvalidPaymentException : BadRequestException
    {
        public InvalidPaymentException(Exception e) : base(e)
        {
        }

        public InvalidPaymentException(string message) : base(message)
        {
        }
    }
}