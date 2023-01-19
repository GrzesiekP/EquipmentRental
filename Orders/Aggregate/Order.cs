using System;
using Core.Domain.Aggregates;
using Core.Domain.Events;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;

namespace Orders.Aggregate
{
    public class Order : Aggregate<Guid>, IAggregate
    {
        public OrderStatus Status { get; private set; }
        public string ClientEmail { get; private set; }
        public OrderData OrderData { get; private set; }
        public OrderPayment OrderPayment { get; private set; }

        // Required for event store
        // ReSharper disable once UnusedMember.Global
        public Order()
        {
            Log("Initializing with empty constructor");
        }

        public static Order Submit(Guid orderId, OrderData orderData, string clientEmail)
        {
            Log("initializing on Submit");
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
            Log($"handling {nameof(RequestOrderApproval)}");
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
            Log($"handling {nameof(ApproveOrder)}");
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

        public void ConfirmPayment(ConfirmOrderPayment command)
        {
            if (OrderPayment.IsEnoughForFullPayment(command.Amount))
            {
                var orderFullyPaid = new OrderFullyPaid(command.Amount);
                PublishEvent(orderFullyPaid);
                Apply(orderFullyPaid);
            }
            else
            {
                var orderPartiallyPaid = new OrderPartiallyPaid(command.Amount);
                PublishEvent(orderPartiallyPaid);
                Apply(orderPartiallyPaid);
            }
        }

        private void PublishEvent(IEvent eventToPublish)
        {
            Log($"publishing {eventToPublish.GetType().Name}");
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
            OrderPayment = new OrderPayment(OrderData.TotalPrice);
            
            // First time aggregate is not saved, so the 2nd time is the correct apply.
            LogApplyMethod(orderSubmitted);
        }
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Version++;
            
            Status = OrderStatus.WaitingForApproval;
            
            LogApplyMethod(approvalRequested);
        }
        
        private void Apply(OrderApproved orderApproved)
        {
            Version++;
            
            Status = OrderStatus.Approved;
            
            Console.WriteLine($"{nameof(Order)}: {nameof(OrderApproved)}. Order:{Id}");
        }
        
        private void Apply(OrderFullyPaid orderFullyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderFullyPaid.Amount);
            Status = OrderStatus.Paid;
            
            Console.WriteLine($"{nameof(Order)}: {nameof(OrderFullyPaid)}. Order:{Id}");
        }
        
        private void Apply(OrderPartiallyPaid orderFullyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderFullyPaid.Amount);
            Status = OrderStatus.PartiallyPaid;
            
            LogApplyMethod(orderFullyPaid);
        }

        private void LogApplyMethod(IEvent e)
        {
            Log($"{e.GetType().Name}. Order:{Id}");
        }
        #endregion

        private static void Log(string message)
        {
            Console.WriteLine($"{nameof(Order)}: {message}");
        }
    }
}