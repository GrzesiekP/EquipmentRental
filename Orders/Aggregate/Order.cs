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
        
        public void Submit(SubmitOrder submitOrder)
        {
            var orderSubmittedEvent = new OrderSubmitted(submitOrder.OrderId);
            
            PublishEvent(orderSubmittedEvent);
            Apply(orderSubmittedEvent);
        }

        public static Order Initialize(Guid orderId)
        {
            var order = new Order(orderId);
            return order;
        }

        private Order(Guid id)
        {
            var orderInitialized = new OrderInitialized(id);
            
            PublishEvent(orderInitialized);
            Apply(orderInitialized);
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

        private void Apply(OrderInitialized orderInitialized)
        {
            Id = orderInitialized.OrderId;
            
            Console.WriteLine($"{nameof(OrderInitialized)}. Order:{Id}");
        }
        
        private void Apply(OrderSubmitted orderSubmitted)
        {
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