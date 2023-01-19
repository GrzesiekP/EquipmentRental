using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;

namespace Orders.Tests.Projections.Scenarios;

[TestClass]
public class WhenOrderApproved : OrderInfoTestsBase
{
    protected override void When()
    {
        base.When();

        var orderApproved = new OrderApproved(OrderId);
        Projection.When(orderApproved);
    }

    [TestMethod]
    public void ThenStatusIsUpdated()
    {
        Assert.AreEqual(OrderStatus.Approved, Projection.Status);
    }
}