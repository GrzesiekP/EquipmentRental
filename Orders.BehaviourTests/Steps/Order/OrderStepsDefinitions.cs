using Orders.Models.Entities;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

[Binding]
public partial class OrderStepsDefinitions
{
    private ScenarioContext _scenarioContext;
    private string _userEmail = "user@example.com";
    private RentalPeriod _rentalPeriod = new(DateTime.Today, DateTime.Today.AddDays(1));
    private List<EquipmentItem> _equipment = new()
    {
        new EquipmentItem(new EquipmentType("NEVIS", new Money(20)))
    };
    private OrderData _orderData;
    private Aggregate.Order _order;
    
    public OrderStepsDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        InitializeOrderWithDefaultValues();
    }
    
    private void InitializeOrderWithDefaultValues()
    {
        _orderData = new OrderData(_equipment, _rentalPeriod);
        _order = InitializeAggregate();
    }
    
    private Aggregate.Order InitializeAggregate()
    {
        var orderId  = Guid.NewGuid();
        var aggregate = Aggregate.Order.Submit(orderId, _orderData, _userEmail);

        return aggregate;
    }
}