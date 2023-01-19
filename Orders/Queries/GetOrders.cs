using System.Collections.Generic;
using Core.Domain.Queries;
using Orders.Projections;

namespace Orders.Queries
{
    public class GetOrders : IQuery<IEnumerable<OrderInfo>>
    {
        public GetOrders(string clientEmail)
        {
            ClientEmail = clientEmail;
        }
        
        public string ClientEmail { get; private set; }
    }
}