using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;

namespace Orders.Tests.Projections.Scenarios;

[TestClass]
public class WhenOrderSubmitted : OrderInfoTestsBase
{
    protected override void When()
    {
        base.When();

        var orderSubmitted = new OrderSubmitted(
            OrderId, 
            OrderData,
            ClientEmail
        );
        Projection.When(orderSubmitted);
    }

    [TestMethod]
    public void ThenOrderIdIsUpdated()
    {
        Assert.AreEqual(OrderId, Projection.Id);
    }
    
    [TestMethod]
    public void ThenOrderDataIsUpdated()
    {
        Assert.AreEqual(OrderData, Projection.OrderData);
    }
    
    [TestMethod]
    public void ThenClientEmailIsUpdated()
    {
        Assert.AreEqual(ClientEmail, Projection.ClientEmail);
    }
    
    [TestMethod]
    public void ThenStatusIsUpdated()
    {
        Assert.AreEqual(OrderStatus.Submitted, Projection.Status);
    }
}