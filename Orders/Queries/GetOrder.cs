using System;
using Core.Domain.Queries;
using Orders.Projections;

namespace Orders.Queries
{
    public class GetOrder : IQuery<OrderInfo>
    {
        public GetOrder(Guid orderId, string clientEmail)
        {
            ClientEmail = clientEmail;
            OrderId = orderId;
        }
        
        public string ClientEmail { get; set; }
        public Guid OrderId { get; private set; }
    }
}