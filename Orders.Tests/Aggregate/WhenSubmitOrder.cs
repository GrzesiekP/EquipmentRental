using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;
using Orders.ValueObjects;

namespace Orders.Tests.Aggregate;

[TestClass]
public class WhenSubmitOrder : AggregateTestsBase
{
    private OrderSubmitted? _orderSubmitted;

    protected override void When()
    {
        var e = GetAggregateEvents().SingleOrDefault(e => e.GetType() == typeof(OrderSubmitted));
        _orderSubmitted = (OrderSubmitted)e!;
        base.When();
    }

    [TestMethod]
    public void ThenStatusIsSubmitted()
    {
        Assert.AreEqual(OrderStatus.Submitted, Order.Status);
    }
    
    [TestMethod]
    public void ThenOrderDataIsPopulated()
    {
        CollectionAssert.AreEquivalent(OrderData.EquipmentItems, Order.OrderData.EquipmentItems);
    }
    
    [TestMethod]
    public void ThenVersionIsUpdated()
    {
        Assert.IsTrue(Order.Version > 0);
    }

    [TestMethod]
    public void ThenOrderSubmittedPublished()
    {
        Assert.IsNotNull(_orderSubmitted);
        Assert.AreEqual(OrderId, _orderSubmitted.OrderId);
    }
}