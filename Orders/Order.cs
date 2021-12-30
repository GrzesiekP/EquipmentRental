using System;
using Core;
using Orders.Events;

namespace Orders
{
    public class Order : Aggregate<Guid>, IAggregate
    {
        public OrderStatus Status { get; private set; }
        
        public static Order Submit(Guid orderId)
        {
            var order = new Order(orderId);
            return order;
        }
        
        private Order(Guid id)
        {
            var orderSubmittedEvent = new OrderSubmitted(id);
            
            PublishEvent(orderSubmittedEvent);
            Apply(orderSubmittedEvent);
        }

        private void PublishEvent(IEvent eventToPublish)
        {
            Enqueue(eventToPublish);
        }

        private void Apply(OrderSubmitted orderSubmitted)
        {
            Id = orderSubmitted.OrderId;
            Status = OrderStatus.Submitted;
        }
    }
}