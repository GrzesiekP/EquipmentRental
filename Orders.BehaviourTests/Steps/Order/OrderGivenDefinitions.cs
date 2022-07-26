using Orders.Commands;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;


public sealed partial class OrderStepsDefinitions
{
    [Given("the user email is (.*)")]
    public void GivenTheUserEmailIs(string email)
    {
        _userEmail = email;
    }

    [Given("the rental period is (.*) days from today")]
    public void GivenTheRentalPeriodIs(int rentalDays)
    {
        _rentalPeriod = new RentalPeriod(DateTime.Today, DateTime.Today.AddDays(rentalDays));
    }

    [Given("the total equipment price is (.*) per day")]
    public void GivenTheEquipmentIsWorthDaily(decimal dailyPrice)
    {
        _equipment = new List<EquipmentItem>
        {
            new EquipmentItem(new EquipmentType("EQ1", new Money(dailyPrice)))
        };
    }
    
    [Given(@"there is (.*) equipments of type (.*) which rental price is (.*)")]
    public void GivenEquipmentTypeIs(int numberOfEquipment, string type, decimal price)
    {
        var equipmentType = new EquipmentType(type, new Money(price));
        for (var i = 0; i < numberOfEquipment; i++)
        {
            _equipment.Add(new EquipmentItem(equipmentType));
        }
    }
    
    [Given(@"the approval is requested for order")]
    public void GivenTheApprovalIsRequested()
    {
        _order.RequestApproval(new RequestOrderApproval(_order.Id));
    }
    
    [Given(@"the order is approved")]
    public void GivenTheOrderIsApproved()
    {
        GivenTheApprovalIsRequested();
        _order.Approve(new ApproveOrder(_order.Id));
    }

    [Given(@"the order is fully paid")]
    public void GivenTheOrderIsFullyPaid()
    {
        GivenTheOrderIsApproved();
        _order.PayOrder(new PayOrder(_order.Id, _order.OrderPayment.TotalMoney));
    }
        
    [Given(@"the order is reserved")]
    public void GivenTheOrderIsReserved()
    {
        GivenTheOrderIsFullyPaid();
        _order.ReserveEquipment(new ReserveEquipment());
    }
    
    [Given(@"the order is in realisation")]
    public void GivenTheOrderIsInRealisation()
    {
        GivenTheOrderIsReserved();
        _order.RentEquipment(new RentEquipment());
    }
}