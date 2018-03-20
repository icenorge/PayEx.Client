using System.Collections.Generic;

namespace PayEx.Client.Models.Vipps.TransactionAPI.Response
{
    public class TransactionListContainer
    {
        public string Id { get; set; }
        public List<TransactionResponse> TransactionList { get; set; } = new List<TransactionResponse>();
    }

    public class AuthorizationListContainer
    {
        public string Id { get; set; }
        public List<TransactionContainerResponse> AuthorizationList { get; set; } = new List<TransactionContainerResponse>();
    }

    public class CapturesListContainer
    {
        public string Id { get; set; }
        public List<TransactionContainerResponse> CaptureList { get; set; } = new List<TransactionContainerResponse>();
    }

    public class ReversalsListContainer
    {
        public string Id { get; set; }
        public List<TransactionContainerResponse> ReversalList { get; set; } = new List<TransactionContainerResponse>();
    }
    
    public class CancellationsListContainer
    {
        public string Id { get; set; }
        public List<TransactionContainerResponse> CancellationList { get; set; } = new List<TransactionContainerResponse>();
    }

    public class TransactionContainerResponse
    {
        public string Id { get; set; }
        public TransactionResponse Transaction { get; set; }
    }
}