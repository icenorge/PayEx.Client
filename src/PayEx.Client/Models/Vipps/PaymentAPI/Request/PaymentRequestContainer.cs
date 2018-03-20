namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    public class PaymentRequestContainer
    {
        public PaymentRequestContainer(PaymentRequest payment)
        {
            Payment = payment;
        }
        public PaymentRequest Payment { get; set; }
    }
}