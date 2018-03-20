namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    public class PaymentAbortRequest
    {
        public string Operation { get; set; } = "Abort";
        public string AbortReason { get; set; } = "CancelledByCustomer";
    }
}