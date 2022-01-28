using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using IEvent = Core.Domain.Events.IEvent;

namespace Orders.Tests.Aggregate;

[TestClass]
public class AggregateTests
{
    private Guid _id;

    private Order InitializeAggregate()
    {
        _id = Guid.NewGuid();
        var aggregate = Order.Submit(_id);

        return aggregate;
    }

    private static IEnumerable<IEvent> GetAggregateEvents(Order aggregate) => aggregate.DequeueUncommittedEvents();

    [TestMethod]
    public void SubmitOrderTest()
    {
        // given
        var order = InitializeAggregate();
        
        // then status and version updated
        Assert.AreEqual(order.Id, _id);
        Assert.AreEqual(order.Status, OrderStatus.Submitted);
        Assert.IsTrue(order.Version > 0);

        // then OrderSubmitted published
        var e = GetAggregateEvents(order).SingleOrDefault(e => e.GetType() == typeof(OrderSubmitted));
        Assert.IsNotNull(e);
        var orderSubmitted = (OrderSubmitted)e;
        Assert.AreEqual(_id, orderSubmitted.OrderId);
    }

    [TestMethod]
    public void RequestApprovalTest()
    {
        // given
        var order = InitializeAggregate();
        var initialVersion = order.Version;

        // when
        var requestApproval = new RequestApproval(_id);
        order.RequestApproval(requestApproval);
        
        // then status and version updated
        Assert.AreEqual(order.Id, _id);
        Assert.IsTrue(order.Status == OrderStatus.WaitingForApproval);
        Assert.AreEqual(1, order.Version - initialVersion);
        
        // then ApprovalRequested published
        var e = GetAggregateEvents(order).SingleOrDefault(e => e.GetType() == typeof(ApprovalRequested));
        Assert.IsNotNull(e);
        var approvalRequested = (ApprovalRequested)e;
        Assert.AreEqual(_id, approvalRequested.OrderId);
    }
    
    [TestMethod]
    public void RequestApprovalFailTest()
    {
        // given
        var order = InitializeAggregate();
        var firstRequestApproval = new RequestApproval(_id);
        order.RequestApproval(firstRequestApproval);

        // when
        var requestApproval = new RequestApproval(_id);

        // then throws exception
        Assert.ThrowsException<Exception>(() => order.RequestApproval(requestApproval));
        
        // then second event is not published
        var e = GetAggregateEvents(order).SingleOrDefault(e => e.GetType() == typeof(ApprovalRequested));
        Assert.IsNotNull(e);
    }
}