using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;

namespace Orders.Tests.Aggregate;

[TestClass]
public class WhenApprovalRequested : AggregateTestsBase<ApprovalRequested>
{
    private ApprovalRequested? _approvalRequested;

    protected override void When()
    {
        var requestApproval = new RequestOrderApproval(OrderId);
        Order.RequestApproval(requestApproval);
        
        _approvalRequested = GetEvent();
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