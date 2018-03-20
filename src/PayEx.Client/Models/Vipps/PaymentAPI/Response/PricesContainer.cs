using System.Collections.Generic;
using PayEx.Client.Models.Vipps.PaymentAPI.Common;
using PayEx.Client.Models.Vipps.PaymentAPI.Request;

namespace PayEx.Client.Models.Vipps.PaymentAPI.Response
{
    public class PricesContainer    
    {
        public List<Price> PriceList { get; set; } = new List<Price>();
    }
}