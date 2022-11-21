using Orders.Commands;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [When("order is submitted")]
    public void WhenOrderIsSubmitted()
    {
        _orderData = new OrderData(_equipment, _rentalPeriod, _totalPrice);
        _action = () => _order = InitializeAggregate();
    }
    
    [When("order is approved")]
    public void WhenOrderIsApproved()
    {
        _action = () => _order.Approve(new ApproveOrder(_order.Id));
    }
}