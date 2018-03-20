namespace PayEx.Client.Models.Vipps.TransactionAPI.Response
{
    internal class CaptureTransactionResponseContainer
    {
        public string Payment { get; set; }
        public TransactionContainer Capture { get; set; }
    }
}