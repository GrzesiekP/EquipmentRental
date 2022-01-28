using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;

namespace Orders.Tests.Aggregate;

[TestClass]
public class WhenRequestApproval : AggregateTestsBase
{
    private ApprovalRequested? _approvalRequested;

    protected override void When()
    {
        var requestApproval = new RequestApproval(OrderId);
        Order.RequestApproval(requestApproval);
        
        var e = GetAggregateEvents().SingleOrDefault(e => e.GetType() == typeof(ApprovalRequested));
        _approvalRequested = (ApprovalRequested)e!;
        
        base.When();
    }

    [TestMethod]
    public void ThenStatusIsWaitingForApproval()
    {
        Assert.AreEqual(OrderStatus.WaitingForApproval, Order.Status);
    }
    
    [TestMethod]
    public void ThenVersionIsUpdated()
    {
        Assert.IsTrue(Order.Version > InitialVersion);
    }

    [TestMethod]
    public void ThenApprovalRequestedPublished()
    {
        Assert.IsNotNull(_approvalRequested);
        Assert.AreEqual(OrderId, _approvalRequested.OrderId);
    }
}