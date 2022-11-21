using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;

namespace Orders.Tests.Aggregate.Submitting;

[TestClass]
public class WhenSubmittingOrder : AggregateTestsBase<OrderSubmitted>
{
    private OrderSubmitted? _orderSubmitted;

    protected override void When()
    {
        _orderSubmitted = GetEvent();
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
        Assert.AreEqual(UserEmail, Order.ClientEmail);
    }
    
    [TestMethod]
    public void ThenOrderDataIsPopulated()
    {
        Assert.IsNotNull(OrderData);
        foreach (var equipmentType in Order.OrderData.EquipmentItems.Keys)
        {
            Assert.AreEqual(OrderData.EquipmentItems[equipmentType], Order.OrderData.EquipmentItems[equipmentType]);
        }
    }
    
    [TestMethod]
    public void ThenRentalDatesArePopulated()
    {
        Assert.AreEqual(OrderData?.RentalPeriod, Order.OrderData.RentalPeriod);
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