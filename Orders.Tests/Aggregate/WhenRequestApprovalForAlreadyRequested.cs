using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Commands;
using Orders.Events;

namespace Orders.Tests.Aggregate;

[TestClass]
public class WhenRequestApprovalForAlreadyRequested : AggregateTestsBase
{
    private ApprovalRequested? _approvalRequested;

    protected override void Given()
    {
        base.Given();
        
        var requestApproval = new RequestOrderApproval(OrderId);
        Order.RequestApproval(requestApproval);
    }

    protected override void When()
    {
        base.When();
        
        var e = GetAggregateEvents().SingleOrDefault(e => e.GetType() == typeof(ApprovalRequested));
        _approvalRequested = (ApprovalRequested)e!;
    }

    [TestMethod]
    public void ThrowsException()
    {
        var requestApproval = new RequestOrderApproval(OrderId);
        Assert.ThrowsException<Exception>(() => Order.RequestApproval(requestApproval));
    }
    
    [TestMethod]
    public void ThenSecondEventIsNotPublished()
    {
        Assert.IsNotNull(_approvalRequested);
    }
}