using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using Orders.Models.ValueObjects;

namespace Orders.Tests.Aggregate.Paying;

[TestClass]
public class WhenSubmittingPartialPayment : AggregateTestsBase<OrderPartiallyPaid>
{
    private OrderPartiallyPaid? _orderPartiallyPaid;

    private Money _paymentAmount;

    protected override void Given()
    {
        base.Given();

        var halfOfAmount = Order.OrderData.TotalPrice / 2;
        _paymentAmount = halfOfAmount;
    }

    protected override void When()
    {
        var payOrder = new ConfirmOrderPayment(OrderId, _paymentAmount);
        Order.ConfirmPayment(payOrder);
        
        _orderPartiallyPaid = GetEvent();
    }

    [TestMethod]
    public void ThenStatusIsPartiallyPaid()
    {
        Assert.AreEqual(OrderStatus.PartiallyPaid, Order.Status);
    }
    
    [TestMethod]
    public void ThenVersionIsUpdated()
    {
        Assert.IsTrue(Order.Version > InitialVersion);
    }

    [TestMethod]
    public void ThenOrderPartiallyPaidPublished()
    {
        Assert.IsNotNull(_orderPartiallyPaid);
    }

    [TestMethod]
    public void ThenPaidAmountIsCorrect()
    {
        Assert.AreEqual(Order.OrderPayment.PaidMoney, _orderPartiallyPaid?.Amount);
    }
}