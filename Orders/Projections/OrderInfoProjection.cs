using Marten.Events.Aggregation;
using Orders.Events;

namespace Orders.Projections
{
    public class OrderInfoProjection : AggregateProjection<OrderInfo>
    {
        public OrderInfoProjection()
        {
            ProjectEvent<OrderSubmitted>((orderInfo, e) => orderInfo.Apply(e));

            ProjectEvent<ApprovalRequested>((orderInfo, e) => orderInfo.Apply(e));

            ProjectEvent<OrderApproved>((orderInfo, e) => orderInfo.Apply(e));
        }
    }
}