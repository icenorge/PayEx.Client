namespace PayEx.Client.Models.Vipps.PaymentAPI.Common
{
    public class Price
    {
        public string Type { get; set; }
        public long Amount { get; set; }
        public long VatAmount { get; set; }
    }
}