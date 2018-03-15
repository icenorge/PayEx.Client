namespace PayEx.Client.Models.Vipps.TransactionAPI.Response
{
    public class TransactionResponse
    {
        public string Id { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public long Amount { get; set; }
        public long VatAmount { get; set; }
        public string Description { get; set; }
        public string PayeeReference { get; set; }
        public string FailedReason { get; set; }
    }
}
