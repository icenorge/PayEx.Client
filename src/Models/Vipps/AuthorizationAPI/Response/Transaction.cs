namespace PayEx.Client.Models.Vipps.AuthorizationAPI.Response
{
    public class Transaction
    {
        public string Id { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public long Amount { get; set; }
        public long VatAmount { get; set; }
        public string Description { get; set; }
        public string PayeeReference { get; set; }
        public string FailedReason { get; set; }
    }
}