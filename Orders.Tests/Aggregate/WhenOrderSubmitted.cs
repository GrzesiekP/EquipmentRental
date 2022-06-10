using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;

namespace Orders.Tests.Aggregate;

[TestClass]
public class WhenOrderSubmitted : AggregateTestsBase
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
    public void ThenClientEmailIsPopulated()
    {
        Assert.IsNotNull(Order.ClientEmail);
        Assert.AreEqual(ClientEmail, Order.ClientEmail);
    }
    
    [TestMethod]
    public void ThenOrderDataIsPopulated()
    {
        Assert.IsNotNull(OrderData);
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