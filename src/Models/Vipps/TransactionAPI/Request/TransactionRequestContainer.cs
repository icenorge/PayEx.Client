namespace PayEx.Client.Models.Vipps.TransactionAPI.Request
{
    internal class TransactionRequestContainer
    {
        public TransactionRequest Transaction { get; }

        public TransactionRequestContainer(TransactionRequest transaction)
        {
            Transaction = transaction;
        }
    }
}