using System;
using System.Linq;
using Core.Domain.Aggregates;
using Core.Domain.Events;
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
        private AggregateLogger<Order, Guid> Logger => new(Id);

        public OrderStatus Status { get; private set; }
        public string ClientEmail { get; private set; }
        public OrderData OrderData { get; private set; }
        public OrderPayment OrderPayment { get; private set; }

        // ReSharper disable once UnusedMember.Global - Required for event store
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

            IEvent paymentEvent;
            Action<IEvent> applyPaymentEvent;
            if (OrderPayment.IsEnoughForFullPayment(confirmOrderPayment.Amount))
            {
                paymentEvent = new OrderFullyPaid(confirmOrderPayment.Amount);
                applyPaymentEvent = e => Apply((OrderFullyPaid)e);
            }
            else
            {
                paymentEvent = new OrderPartiallyPaid(confirmOrderPayment.Amount);
                applyPaymentEvent = e => Apply((OrderPartiallyPaid)e);
            }
            
            Logger.LogPublishEvent(paymentEvent);
                
            PublishEvent(paymentEvent);
            applyPaymentEvent(paymentEvent);
        }
        
        public void ReserveEquipment(ReserveEquipment command)
        {
            if (!OrderData.EquipmentItems.All(e => e.IsAvailableFor(OrderData.RentalPeriod)))
            {
                var unavailableEquipmentIds = OrderData.EquipmentItems
                    .Where(e => !e.IsAvailableFor(OrderData.RentalPeriod))
                    .Select(e => e.Identity);
                throw new AggregateException(
                    $"Reservation failed. Following equipment is not available: {string.Join(',', unavailableEquipmentIds)}");
            }
            
            var equipmentReserved = new EquipmentReserved(Id, OrderData.RentalPeriod);
                
            PublishEvent(equipmentReserved);
            Apply(equipmentReserved);
        }
        
        public void ConfirmEquipmentRent(ConfirmEquipmentRent command)
        {
            var equipmentRent = new EquipmentRent(Id);
            
            Logger.LogPublishEvent(equipmentRent);
            
            PublishEvent(equipmentRent);
            Apply(equipmentRent);
        }
        
        public void ConfirmEquipmentReturned(ConfirmEquipmentReturned command)
        {
            var equipmentReturned = new EquipmentReturned(Id);
            
            Logger.LogPublishEvent(equipmentReturned);
            
            PublishEvent(equipmentReturned);
            Apply(equipmentReturned);
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
            Logger.LogApplyMethod(orderSubmitted);
        }
        
        private void Apply(ApprovalRequested approvalRequested)
        {
            Version++;
            
            Status = OrderStatus.WaitingForApproval;
            
            Logger.LogApplyMethod(approvalRequested);
        }
        
        private void Apply(OrderApproved orderApproved)
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
                equipmentItem.ReserveFor(equipmentReserved.Period);
            }

            Status = OrderStatus.Reserved;
            
            Logger.LogApplyMethod(equipmentReserved);
        }
        
        private void Apply(EquipmentRent equipmentRent)
        {
            Version++;

            Status = OrderStatus.InRealisation;
            
            foreach (var equipmentItem in OrderData.EquipmentItems)
            {
                equipmentItem.Rent();
            }
            
            Logger.LogApplyMethod(equipmentRent);
        }
        
        private void Apply(EquipmentReturned equipmentReturned)
        {
            Version++;

            Status = OrderStatus.Completed;
            
            foreach (var equipmentItem in OrderData.EquipmentItems)
            {
                equipmentItem.Return();
            }
            
            Logger.LogApplyMethod(equipmentReturned);
        }
        
        #endregion
    }
}