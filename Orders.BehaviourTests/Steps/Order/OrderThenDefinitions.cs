using FluentAssertions;
using Orders.Aggregate.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [Then("order status is (.*)")]
    public void ThenOrderShouldBeInStatus(OrderStatus status)
    {
        _order.Status.Should().Be(status);
    }
    
    [Then("order data should be the same as in input")]
    public void ThenOrderDataShouldBeTheSameAsInInput()
    {
        _order.OrderData.Should().BeEquivalentTo(_orderData);
    }

    [Then(@"order equipment is reserved for order reservation period")]
    public void ThenOrderEquipmentIsReservedForOrderReservationPeriod()
    {
        _order.OrderData.EquipmentItems.Should().Satisfy(e => e.IsReservedFor(_rentalPeriod));
    }
    
    [Then(@"order equipment is not reserved")]
    public void ThenOrderEquipmentIsNotReserved()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"equipment is (.*)")]
    public void ThenEquipmentIsRent(EquipmentStatus equipmentStatus)
    {
        _order.OrderData.EquipmentItems.Should().Satisfy(e => e.Status == equipmentStatus);
    }

    [Then(@"exception is thrown")]
    public void ThenExceptionIsThrown()
    {
        _action.Should().Throw<Exception>();
    }
}