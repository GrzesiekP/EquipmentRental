using FluentAssertions;
using Orders.Aggregate.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [Then("order status is (.*)")]
    public void ThenOrderStatusIs(OrderStatus status)
    {
        _order.Status.Should().Be(status);
    }
    
    [Then("order data should be the same as in input")]
    public void ThenOrderDataShouldBeTheSameAsInInput()
    {
        _order.OrderData.Should().BeEquivalentTo(_orderData);
    }

    [Then(@"there is no exception")]
    public void ThenThereIsNoException()
    {
        _action();
    }

    [Then(@"exception is thrown")]
    public void ThenExceptionIsThrown()
    {
        _action.Should().Throw<Exception>();
    }
}