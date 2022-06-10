using System;
using Core.Domain.Aggregates;
using Core.Domain.Events;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using Orders.ValueObjects;

namespace Orders.Aggregate
{
    public class Order : Aggregate<Guid>, IAggregate
    {
        public OrderStatus Status { get; private set; }
        public string ClientEmail { get; private set; }
        public OrderData OrderData { get; private set; }

        // Required for event store
        // ReSharper disable once UnusedMember.Global
        public Order()
        {
        }

        public static Order Submit(Guid orderId, OrderData orderData, string clientEmail)
        {
            var order = new Order(orderId, orderData, clientEmail);
            return order;
        }

        private Order(Guid id, OrderData orderData, string clientEmail)
        {
            var orderSubmitted = new OrderSubmitted(id, orderData, clientEmail);
            
            PublishEvent(orderSubmitted);
            Apply(orderSubmitted);
        }

        public void RequestApproval(RequestOrderApproval requestOrderApproval)
        {
            if (Status != OrderStatus.Submitted)
            {
                throw new Exception(
                    $"Cannot request for approval for OrderId:{requestOrderApproval.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var approvalRequested = new ApprovalRequested(requestOrderApproval.OrderId);

            PublishEvent(approvalRequested);
            Apply(approvalRequested);
        }

        public void Approve(ApproveOrder approveOrder)
        {
            if (Status != OrderStatus.WaitingForApproval)
            {
                throw new Exception(
                    $"Cannot approve order OrderId:{approveOrder.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var requestApproved = new OrderApproved(approveOrder.OrderId);
            
            PublishEvent(requestApproved);
            Apply(requestApproved);
        }

        private void PublishEvent(IEvent eventToPublish)
        {
            Enqueue(eventToPublish);
        }

        #region Apply
        private void Apply(OrderSubmitted orderSubmitted)
        {
            Version++;

            Id = orderSubmitted.OrderId;
            ClientEmail = orderSubmitted.ClientEmail;
            Status = OrderStatus.Submitted;
            OrderData = orderSubmitted.OrderData;
            
            // First time aggregate is not saved, so the 2nd time is the correct apply.
            Console.WriteLine($"{nameof(OrderSubmitted)}. Order:{Id}");
        }
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Version++;
            
            Status = OrderStatus.WaitingForApproval;
            
            Console.WriteLine($"{nameof(ApprovalRequested)}. Order:{Id}");
        }
        
        private void Apply(OrderApproved orderApproved)
        {
            Version++;
            
            Status = OrderStatus.Approved;
            
            Console.WriteLine($"{nameof(OrderApproved)}. Order:{Id}");
        }
        #endregion
    }
}