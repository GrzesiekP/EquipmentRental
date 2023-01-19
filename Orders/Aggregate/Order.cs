using System;
using Core.Domain.Aggregates;
using Core.Logging;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;

namespace Orders.Aggregate
{
    public class Order : Aggregate<Guid>, IAggregate
    {
        private readonly AggregateLogger<Order, Guid> _logger;

        public OrderStatus Status { get; private set; }
        public string ClientEmail { get; private set; }
        public OrderData OrderData { get; private set; }
        public OrderPayment OrderPayment { get; private set; }

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
            _logger = new AggregateLogger<Order, Guid>(id);
            
            var orderSubmitted = new OrderSubmitted(id, orderData, clientEmail);
            
            PublishEvent(orderSubmitted);
            Apply(orderSubmitted);
        }

        public void RequestApproval(RequestOrderApproval requestOrderApproval)
        {
            _logger.LogCommand(requestOrderApproval);
            
            if (Status != OrderStatus.Submitted)
            {
                throw new Exception(
                    $"Cannot request for approval for OrderId:{requestOrderApproval.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var approvalRequested = new ApprovalRequested(requestOrderApproval.OrderId);

            _logger.LogPublishEvent(approvalRequested);
            
            PublishEvent(approvalRequested);
            Apply(approvalRequested);
        }

        public void Approve(ApproveOrder approveOrder)
        {
            _logger.LogCommand(approveOrder);
            
            if (Status != OrderStatus.WaitingForApproval)
            {
                throw new Exception(
                    $"Cannot approve order OrderId:{approveOrder.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var requestApproved = new OrderApproved(approveOrder.OrderId);
            
            _logger.LogPublishEvent(requestApproved);
            
            PublishEvent(requestApproved);
            Apply(requestApproved);
        }

        public void ConfirmPayment(ConfirmOrderPayment confirmOrderPayment)
        {
            _logger.LogCommand(confirmOrderPayment);
            
            if (OrderPayment.IsEnoughForFullPayment(confirmOrderPayment.Amount))
            {
                var orderFullyPaid = new OrderFullyPaid(confirmOrderPayment.Amount);
                
                _logger.LogPublishEvent(orderFullyPaid);
                
                PublishEvent(orderFullyPaid);
                Apply(orderFullyPaid);
            }
            else
            {
                var orderPartiallyPaid = new OrderPartiallyPaid(confirmOrderPayment.Amount);
                
                _logger.LogPublishEvent(orderPartiallyPaid);
                
                PublishEvent(orderPartiallyPaid);
                Apply(orderPartiallyPaid);
            }
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
            _logger.LogApplyMethod(orderSubmitted);
        }
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Version++;
            
            Status = OrderStatus.WaitingForApproval;
            
            _logger.LogApplyMethod(approvalRequested);
        }
        
        private void Apply(OrderApproved orderApproved)
        {
            Version++;
            
            Status = OrderStatus.Approved;
            
            _logger.LogApplyMethod(orderApproved);
        }
        
        private void Apply(OrderFullyPaid orderFullyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderFullyPaid.Amount);
            Status = OrderStatus.Paid;
            
            _logger.LogApplyMethod(orderFullyPaid);
        }
        
        private void Apply(OrderPartiallyPaid orderPartiallyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderPartiallyPaid.Amount);
            Status = OrderStatus.PartiallyPaid;
            
            _logger.LogApplyMethod(orderPartiallyPaid);
        }
        #endregion
    }
}