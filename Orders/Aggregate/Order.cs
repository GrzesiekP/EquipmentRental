using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Aggregates;
using Core.Logging;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;

namespace Orders.Aggregate
{
    public class Order : Aggregate<Guid>,
        IAggregateConsumer<RequestOrderApproval, ApprovalRequested>,
        IAggregateConsumer<ApproveOrder, OrderApproved>,
        IAggregateConsumer<SubmitOrder, OrderSubmitted>,
        IAggregate
    {
        private AggregateLogger<Order, Guid> Logger => new(Id);

        public OrderStatus Status { get; private set; }
        public string ClientEmail { get; private set; }
        public OrderData OrderData { get; private set; }
        public OrderPayment OrderPayment { get; private set; }

        // Required for event store
        // ReSharper disable once UnusedMember.Global
        public Order()
        {
        }
        
        public IEnumerable<OrderSubmitted> Consume(SubmitOrder command)
        {
            var order = new Order(command.OrderId, command.OrderData, command.ClientEmail);
            var orderSubmitted = new OrderSubmitted(order.Id, order.OrderData, order.ClientEmail);
            yield return orderSubmitted;
        }

        // OLD
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

        // OLD
        public void RequestApproval(RequestOrderApproval requestOrderApproval)
        {
            Logger.LogCommand(requestOrderApproval);
            
            if (Status != OrderStatus.Submitted)
            {
                throw new Exception(
                    $"Cannot request for approval for OrderId:{requestOrderApproval.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var approvalRequested = new ApprovalRequested(requestOrderApproval.OrderId);

            Logger.LogPublishEvent(approvalRequested);
            
            PublishEvent(approvalRequested);
            Apply(approvalRequested);
        }
        
        public IEnumerable<ApprovalRequested> Consume(RequestOrderApproval command)
        {
            if (Status != OrderStatus.Submitted)
            {
                throw new Exception(
                    $"Cannot request for approval for OrderId:{command.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var approvalRequested = new ApprovalRequested(command.OrderId);

            yield return approvalRequested;
        }

        public IEnumerable<OrderApproved> Consume(ApproveOrder approveOrder)
        {
            if (Status != OrderStatus.WaitingForApproval)
            {
                throw new Exception(
                    $"Cannot approve order OrderId:{approveOrder.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var requestApproved = new OrderApproved(approveOrder.OrderId);
            yield return requestApproved;
        }

        // OLD
        public void Approve(ApproveOrder approveOrder)
        {
            Logger.LogCommand(approveOrder);
            
            if (Status != OrderStatus.WaitingForApproval)
            {
                throw new Exception(
                    $"Cannot approve order OrderId:{approveOrder.OrderId}, " +
                    $"because it is in status {Status}");
            }
            
            var requestApproved = new OrderApproved(approveOrder.OrderId);
            
            Logger.LogPublishEvent(requestApproved);
            
            PublishEvent(requestApproved);
            Apply(requestApproved);
        }

        public void ConfirmPayment(ConfirmOrderPayment confirmOrderPayment)
        {
            Logger.LogCommand(confirmOrderPayment);
            
            if (OrderPayment.IsEnoughForFullPayment(confirmOrderPayment.Amount))
            {
                var orderFullyPaid = new OrderFullyPaid(confirmOrderPayment.Amount);
                
                Logger.LogPublishEvent(orderFullyPaid);
                
                PublishEvent(orderFullyPaid);
                Apply(orderFullyPaid);
            }
            else
            {
                var orderPartiallyPaid = new OrderPartiallyPaid(confirmOrderPayment.Amount);
                
                Logger.LogPublishEvent(orderPartiallyPaid);
                
                PublishEvent(orderPartiallyPaid);
                Apply(orderPartiallyPaid);
            }
        }
        
        public void ReserveEquipment(ReserveEquipment command)
        {
            if (OrderData.EquipmentItems.All(e => e.IsAvailableFor(OrderData.RentalPeriod)))
            {
                var equipmentReserved = new EquipmentReserved(Id);
                
                PublishEvent(equipmentReserved);
                Apply(equipmentReserved);
            }

            // notify admin, that reservation failed
        }
        
        public void ConfirmEquipmentRent(ConfirmEquipmentRent command)
        {
            var equipmentRent = new EquipmentRent(Id);
            
            PublishEvent(equipmentRent);
            Apply(equipmentRent);
        }
        
        public void ConfirmEquipmentReturned(ConfirmEquipmentReturned command)
        {
            var equipmentReturned = new EquipmentReturned(Id);
            
            PublishEvent(equipmentReturned);
            Apply(equipmentReturned);
        }

        #region Apply
        public void Apply(OrderSubmitted orderSubmitted)
        {
            Version++;

            Id = orderSubmitted.OrderId;
            ClientEmail = orderSubmitted.ClientEmail;
            Status = OrderStatus.Submitted;
            OrderData = orderSubmitted.OrderData;
            OrderPayment = new OrderPayment(OrderData.TotalPrice);
            
            // First time aggregate is not saved, so the 2nd time is the correct apply.
            Logger.LogApplyMethod(orderSubmitted);
        }
        
        public void Apply(ApprovalRequested approvalRequested)
        {
            Version++;
            
            Status = OrderStatus.WaitingForApproval;
            
            Logger.LogApplyMethod(approvalRequested);
        }
        
        public void Apply(OrderApproved orderApproved)
        {
            Version++;
            
            Status = OrderStatus.Approved;
            
            Logger.LogApplyMethod(orderApproved);
        }
        
        private void Apply(OrderFullyPaid orderFullyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderFullyPaid.Amount);
            Status = OrderStatus.Paid;
            
            Logger.LogApplyMethod(orderFullyPaid);
        }
        
        private void Apply(OrderPartiallyPaid orderPartiallyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderPartiallyPaid.Amount);
            Status = OrderStatus.PartiallyPaid;
            
            Logger.LogApplyMethod(orderPartiallyPaid);
        }
        
        private void Apply(EquipmentReserved equipmentReserved)
        {
            Version++;
            
            foreach (var equipmentItem in OrderData.EquipmentItems)
            {
                equipmentItem.ReserveFor(OrderData.RentalPeriod);
            }

            Status = OrderStatus.Reserved;
            
            Console.WriteLine($"{nameof(EquipmentReserved)}. Order:{Id}");
        }
        
        private void Apply(EquipmentRent equipmentRent)
        {
            Version++;

            Status = OrderStatus.InRealisation;
            
            foreach (var equipmentItem in OrderData.EquipmentItems)
            {
                equipmentItem.Rent();
            }
            
            Console.WriteLine($"{nameof(EquipmentRent)}. Order:{Id}");
        }
        
        private void Apply(EquipmentReturned equipmentReturned)
        {
            Version++;

            Status = OrderStatus.Completed;
            
            foreach (var equipmentItem in OrderData.EquipmentItems)
            {
                equipmentItem.Return();
            }
            
            Console.WriteLine($"{nameof(EquipmentRent)}. Order:{Id}");
        }
        
        #endregion
    }
}