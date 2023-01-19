using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

[Binding]
public partial class OrderStepsDefinitions
{
    private ScenarioContext _scenarioContext;
    private string _userEmail = "user@example.com";
    private RentalPeriod _rentalPeriod = new(DateTime.Today, DateTime.Today.AddDays(1));
    private IDictionary<string, int> _equipment = new Dictionary<string, int>();
    private readonly Money _totalPrice = new Money(20);    
    private OrderData _orderData;
    private Aggregate.Order _order;
    private Action _action;
    
    public OrderStepsDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        InitializeOrderWithDefaultValues();
    }
    
    private void InitializeOrderWithDefaultValues()
    {
        _equipment["NEVIS"] = 1;
        _orderData = new OrderData(_equipment, _rentalPeriod, _totalPrice);
        _order = InitializeAggregate();
    }
    
    private Aggregate.Order InitializeAggregate()
    {
        var orderId  = Guid.NewGuid();
        var aggregate = Aggregate.Order.Submit(orderId, _orderData, _userEmail);

        return aggregate;
    }
}