using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;

namespace Orders.Tests.Aggregate;

[TestClass]
public class WhenApprovingOrder : AggregateTestsBase<OrderApproved>
{
    private OrderApproved? _orderApproved;

    protected override void Given()
    {
        base.Given();
        Order.RequestApproval(new RequestOrderApproval(OrderId));
    }

    protected override void When()
    {
        var approveOrder = new ApproveOrder(OrderId);
        Order.Approve(approveOrder);

        _orderApproved = GetEvent();
    }

    [TestMethod]
    public void ThenStatusIsApproved()
    {
        Assert.AreEqual(OrderStatus.Approved, Order.Status);
    }
    
    [TestMethod]
    public void ThenVersionIsUpdated()
    {
        Assert.IsTrue(Order.Version > InitialVersion);
    }

    [TestMethod]
    public void ThenAOrderApprovedPublished()
    {
        Assert.IsNotNull(_orderApproved);
        Assert.AreEqual(OrderId, _orderApproved.OrderId);
    }
}