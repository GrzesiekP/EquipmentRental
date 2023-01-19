using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Events;
using Orders.Models.ValueObjects;

namespace Orders.Tests.Aggregate.Paying;

[TestClass]
public class WhenSubmittingFullPayment : AggregateTestsBase<OrderFullyPaid>
{
    private OrderFullyPaid? _orderFullyPaid;

    private Money? _fullPaymentAmount;

    protected override void Given()
    {
        base.Given();

        _fullPaymentAmount = Order.OrderData.TotalPrice;
    }

    protected override void When()
    {
        var payOrder = new ConfirmOrderPayment(OrderId, _fullPaymentAmount);
        Order.ConfirmPayment(payOrder);
        
        _orderFullyPaid = GetEvent();
    }

    [TestMethod]
    public void ThenStatusIsPaid()
    {
        Assert.AreEqual(OrderStatus.Paid, Order.Status);
    }
    
    [TestMethod]
    public void ThenVersionIsUpdated()
    {
        Assert.IsTrue(Order.Version > InitialVersion);
    }

    [TestMethod]
    public void ThenOrderFullyPaidPublished()
    {
        Assert.IsNotNull(_orderFullyPaid);
    }

    [TestMethod]
    public void ThenPaidAmountIsCorrect()
    {
        Assert.AreEqual(_fullPaymentAmount, _orderFullyPaid?.Amount);
        Assert.AreEqual(_fullPaymentAmount, Order.OrderPayment.PaidMoney);
    }
}