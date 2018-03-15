namespace PayEx.Client.Models.Vipps.TransactionAPI.Response
{
    internal class CancellationTransactionResponseContainer
    {
        public string Payment { get; set; }
        public TransactionContainer Cancellation { get; set; }
    }
}