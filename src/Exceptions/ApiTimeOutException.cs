using System;
using System.Threading.Tasks;

namespace PayEx.Client.Exceptions
{
    public class ApiTimeOutException : Exception
    {
        public ApiTimeOutException(TaskCanceledException te) : base("Timed out when calling PayEx", te)
        {
            
        }
    }
}