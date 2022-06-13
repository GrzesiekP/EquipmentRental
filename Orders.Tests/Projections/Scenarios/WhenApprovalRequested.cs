using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;

namespace Orders.Tests.Projections.Scenarios;

[TestClass]
public class WhenApprovalRequested : OrderInfoTestsBase
{
    protected override void When()
    {
        base.When();

        var approvalRequested = new ApprovalRequested(OrderId);
        Projection.When(approvalRequested);
    }

    [TestMethod]
    public void ThenStatusIsUpdated()
    {
        Assert.AreEqual(OrderStatus.WaitingForApproval, Projection.Status);
    }
}