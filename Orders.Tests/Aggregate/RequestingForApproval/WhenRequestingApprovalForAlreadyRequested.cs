using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Commands;
using Orders.Events;

namespace Orders.Tests.Aggregate.RequestingForApproval;

[TestClass]
public class WhenRequestingApprovalForAlreadyRequested : AggregateTestsBase<ApprovalRequested>
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
        _approvalRequested = GetEvent();
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