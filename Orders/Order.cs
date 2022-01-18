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
        
        private Order(Guid id)
        {
            var orderSubmittedEvent = new OrderSubmitted(id);
            
            PublishEvent(orderSubmittedEvent);
            Apply(orderSubmittedEvent);
        }

        public void RequestApproval(RequestApproval requestApproval)
        {
            if (Status != OrderStatus.Submitted)
            {
                throw new Exception(
                    $"Cannot request for approval for OrderId:{requestApproval.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var approvalRequested = new ApprovalRequested(requestApproval.OrderId);

            Enqueue(approvalRequested);
            Apply(approvalRequested);
        }

        public void ApproveRequest(ApproveRequest approveRequest)
        {
            if (Status != OrderStatus.WaitingForApproval)
            {
                throw new Exception(
                    $"Cannot approved order OrderId:{approveRequest.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var requestApproved = new RequestApproved(approveRequest.OrderId);
            
            Enqueue(requestApproved);
            Apply(requestApproved);
        }

        private void PublishEvent(IEvent eventToPublish)
        {
            Enqueue(eventToPublish);
        }

        #region Apply
        
        private void Apply(OrderSubmitted orderSubmitted)
        {
            Id = orderSubmitted.OrderId;
            Status = OrderStatus.Submitted;
            
            Console.WriteLine($"{nameof(OrderSubmitted)}. Order:{Id}");
        }
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Status = OrderStatus.WaitingForApproval;
            
            Console.WriteLine($"{nameof(ApprovalRequested)}. Order:{Id}");
        }
        
        private void Apply(RequestApproved requestApproved)
        {
            Status = OrderStatus.Approved;
            
            Console.WriteLine($"{nameof(RequestApproved)}. Order:{Id}");
        }
        #endregion
    }
}