using Core.Models;
using Orders.Commands;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [When("client submits an order")]
    public void WhenClientSubmitsAnOrder()
    {
        _orderData = new OrderData(_equipment, _rentalPeriod);
        _action = () => _order = InitializeAggregate();
    }
    
    [When("admin approves the order")]
    public void WhenAdminApprovesTheOrder()
    {
        _action = () => _order.Approve(new ApproveOrder(_order.Id));
    }
    
    [When(@"admin confirms order payment for (.*)")]
    public void WhenAdminConfirmsOrderPaymentFor(int amount)
    {
        _order.ConfirmPayment(new ConfirmOrderPayment(_order.Id, new Money(amount)));
    }
    
    [When("reserving ordered equipment")]
    public void WhenReservingOrderedEquipment()
    {
        _order.ReserveEquipment(new ReserveEquipment(_order.Id));
    }
    
    [When("admin confirms equipment is rent")]
    public void WhenAdminConfirmsEquipmentIsRent()
    {
        _order.ConfirmEquipmentRent(new ConfirmEquipmentRent(_order.Id));
    }
    
    [When("admin confirms equipment is returned")]
    public void WhenAdminConfirmsEquipmentIsReturned()
    {
        _order.ConfirmEquipmentReturned(new ConfirmEquipmentReturned(_order.Id));
    }
}