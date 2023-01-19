using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Commands;
using Orders.Events;

namespace Orders.Tests.Aggregate.Approving;

[TestClass]
public class WhenApprovingOrderNotWaitingForApproval : AggregateTestsBase<OrderApproved>
{
    private OrderApproved? _orderApproved;
    private Action? _approveOrder;

    protected override void When()
    {
        var approveOrder = new ApproveOrder(OrderId);
        _approveOrder = () => Order.Approve(approveOrder);

        _orderApproved = GetEvent();
    }

    [TestMethod]
    public void ExceptionIsThrown()
    {
        Assert.ThrowsException<Exception>(_approveOrder);
    }
    
    [TestMethod]
    public void ExceptionOrderIsNotApproved()
    {
        Assert.IsNull(_orderApproved);
    }
}