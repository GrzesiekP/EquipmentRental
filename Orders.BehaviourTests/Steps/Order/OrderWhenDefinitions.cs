using Orders.Commands;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [When("client submits an order")]
    public void WhenClientSubmitsAnOrder()
    {
        _orderData = new OrderData(_equipment, _rentalPeriod, _totalPrice);
        _action = () => _order = InitializeAggregate();
    }
    
    [When("admin approves the order")]
    public void WhenAdminApprovesTheOrder()
    {
        _action = () => _order.Approve(new ApproveOrder(_order.Id));
    }
}