using Microsoft.AspNetCore.Http;
using PayEx.Client;

namespace Sample.AspNetCore
{
    public class QueryStringSelector : ISelectClient
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public QueryStringSelector(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        
        public string Select()
        {
            var val = _contextAccessor.HttpContext.Request.Query["selector"];
            return val;
        }
    }
}