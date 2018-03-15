namespace PayEx.Client.Models.Vipps.AuthorizationAPI.Request
{
    public class VippsAuthorizationRequestContainer
    {
        public VippsAuthorizationRequest Transaction { get; }

        public VippsAuthorizationRequestContainer(VippsAuthorizationRequest transaction)
        {
            Transaction = transaction;
        }
    }
}