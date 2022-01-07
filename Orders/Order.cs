using System;
using Core;
using Orders.Commands;
using Orders.Events;

namespace Orders
{
    public class Order : Aggregate<Guid>, IAggregate
    {
        public OrderStatus Status { get; private set; }

        // Required for event store
        // ReSharper disable once UnusedMember.Global
        public Order()
        {
        }
        
        public static Order Submit(Guid orderId)
        {
            var order = new Order(orderId);
            return order;
        }

        public void RequestApproval(RequestApproval requestApproval)
        {
            Console.WriteLine($"Notified about order {requestApproval.OrderId}");
            
            var approvalRequested = new ApprovalRequested(requestApproval.OrderId);

            Enqueue(approvalRequested);
            Apply(approvalRequested);
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
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Status = OrderStatus.WaitingForApproval;
        }
    }
}