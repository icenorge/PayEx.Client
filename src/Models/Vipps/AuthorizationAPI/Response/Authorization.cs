namespace PayEx.Client.Models.Vipps.AuthorizationAPI.Response
{
    public class Authorization
    {
        public string Id { get; set; }
        public string VippsTransactionId { get; set; }
        public string MobileNumber { get; set; }
        public Transaction Transaction { get; set; }
    }
}