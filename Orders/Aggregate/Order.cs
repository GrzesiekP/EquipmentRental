using System;
using Core.Domain;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;

namespace Orders.Aggregate
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
            var orderSubmitted = new OrderSubmitted(id);
            
            PublishEvent(orderSubmitted);
            Apply(orderSubmitted);
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

            PublishEvent(approvalRequested);
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
            Status = OrderStatus.Submitted;
            
            // First time aggregate is not saved, so the 2nd time is the correct apply.
            Console.WriteLine($"{nameof(OrderSubmitted)}. Order:{Id}");
        }
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Version++;
            
            Status = OrderStatus.WaitingForApproval;
            
            Console.WriteLine($"{nameof(ApprovalRequested)}. Order:{Id}");
        }
        
        private void Apply(RequestApproved requestApproved)
        {
            Version++;
            
            Status = OrderStatus.Approved;
            
            Console.WriteLine($"{nameof(RequestApproved)}. Order:{Id}");
        }
        #endregion
    }
}