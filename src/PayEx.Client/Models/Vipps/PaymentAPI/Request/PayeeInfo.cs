namespace PayEx.Client.Models.Vipps.PaymentAPI.Request
{
    public class PayeeInfo
    {
        /// <summary>
        ///     This is the unique id that identifies this payee (like merchant) set by PayEx.
        /// </summary>
        public string PayeeId { get; set; }
       
        /// <summary>
        /// 	A unique reference, max 30 characters, set by the merchant system - this must be unique for each operation!
        ///     NOTE://PayEx may send either the transaction number OR the payeeReference as a reference to the acquirer. 
        ///     This will be used in reconciliation and reporting back to PayEx and you.
        ///     If PayEx sends the transaction number to the acquirer, then the payeeReference parameter may have the format of String(30).
        ///     If PayEx sends the payeeRef to the acquirer, the parameter is limited to the format of String(12) AND all characters must be digits/numbers.
        /// </summary>
        public string PayeeReference { get; set; }

        public string PayeeName { get; set; } 
    }
}