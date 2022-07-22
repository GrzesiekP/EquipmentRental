using System;
using System.Linq;
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
        }

        // Klient: złóż zamówienie
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

        // auto: wyślij prośbę o akceptację
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

        // Admin: potwierdź zamówienie
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
        
        // Admin: zgłoś, że opłacono
        public void PayOrder(PayOrder command)
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
        
        // Auto: Zarezerwuj sprzęt
        public void ReserveEquipment(ReserveEquipment command)
        {
            if (OrderData.EquipmentItems.All(e => e.IsAvailableFor(OrderData.RentalPeriod)))
            {
                var equipmentReserved = new EquipmentReserved();
                
                PublishEvent(equipmentReserved);
                Apply(equipmentReserved);
            }

            // jeśli minął czas na wpłatę powiadom, że sprzętu już nie ma
            
            // jeśli nie minął, throw
        }
        
        // Admin: wydaj sprzęt
        public void RentEquipment(RentEquipment command)
        {
            var equipmentRent = new EquipmentRent();
            
            PublishEvent(equipmentRent);
            Apply(equipmentRent);
        }
        
        // Admin: zgłoś zwrot sprzętu
        public void ReturnEquipment(ReturnEquipment command)
        {
            var equipmentReturned = new EquipmentReturned();
            
            PublishEvent(equipmentReturned);
            Apply(equipmentReturned);
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
            OrderPayment = new OrderPayment(orderSubmitted.OrderData.CalculateTotalPrice());
            
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
        
        private void Apply(OrderFullyPaid orderFullyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderFullyPaid.Amount);
            Status = OrderStatus.Paid;
            
            Console.WriteLine($"{nameof(EquipmentReserved)}. Order:{Id}");
        }
        
        private void Apply(OrderPartiallyPaid orderFullyPaid)
        {
            Version++;
            
            OrderPayment.Pay(orderFullyPaid.Amount);
            Status = OrderStatus.PartiallyPaid;
            
            Console.WriteLine($"{nameof(EquipmentReserved)}. Order:{Id}");
        }
        
        private void Apply(EquipmentReserved equipmentReserved)
        {
            Version++;
            
            foreach (var equipmentItem in OrderData.EquipmentItems)
            {
                equipmentItem.ReserveFor(OrderData.RentalPeriod);
            }
            
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
                equipmentItem.Release();
            }
            
            Console.WriteLine($"{nameof(EquipmentRent)}. Order:{Id}");
        }
        
        #endregion
    }
}