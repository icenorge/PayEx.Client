namespace PayEx.Client.Models.Vipps.TransactionAPI.Response
{
    internal class AllTransactionResponseContainer
    {
        public string Payment { get; set; }
        public TransactionListContainer Transactions { get; set; } 

    }
}