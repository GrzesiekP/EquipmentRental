using System;
using Core.Domain.Projections;
using Orders.Aggregate.ValueObjects;
using Orders.Events;
using Orders.ValueObjects;

namespace Orders.Projections
{
    public class OrderInfo : IProjection
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public OrderData OrderData { get; set; }
        public string ClientEmail { get; set; }
        
        public void When(object e)
        {
            switch (e)
            {
                case OrderSubmitted orderSubmitted:
                    Apply(orderSubmitted);
                    return;
                case ApprovalRequested approvalRequested:
                    Apply(approvalRequested);
                    return;
                case RequestApproved requestApproved:
                    Apply(requestApproved);
                    return;
            }
        }

        public void Apply(OrderSubmitted e)
        {
            Id = e.OrderId;
            Status = OrderStatus.Submitted;
            OrderData = e.OrderData;
            ClientEmail = e.ClientEmail;
        }
        
        public void Apply(ApprovalRequested e)
        {
            Status = OrderStatus.WaitingForApproval;
        }
        
        public void Apply(RequestApproved e)
        {
            Status = OrderStatus.Approved;
        }
    }
}