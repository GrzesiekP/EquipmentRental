using Orders.Commands;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [When("order is submitted")]
    public void WhenOrderIsSubmitted()
    {
        _orderData = new OrderData(_equipment, _rentalPeriod);
        _order = InitializeAggregate();
    }
    
    [When("renting equipment")]
    public void WhenRentingEquipment()
    {
        _order.RentEquipment(new RentEquipment());
    }
    
    [When("reserving ordered equipment")]
    public void WhenReservingEquipment()
    {
        _action = () => _order.ReserveEquipment(new ReserveEquipment());
    }
    
    [When("returning equipment")]
    public void WhenReturningEquipment()
    {
        _order.ReturnEquipment(new ReturnEquipment());
    }
}