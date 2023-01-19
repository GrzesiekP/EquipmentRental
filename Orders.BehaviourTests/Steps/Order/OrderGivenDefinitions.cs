using Core.Models;
using Equipment.Models.Entities;
using Equipment.Models.ValueObjects;
using Orders.Commands;
using Orders.Models.ValueObjects;

namespace Orders.BehaviourTests.Steps.Order;

public sealed partial class OrderStepsDefinitions
{
    [Given("the user email is (.*)")]
    public void GivenTheUserEmailIs(string email)
    {
        _userEmail = email;
    }

    [Given("the order is submitted")]
    private void GivenTheOrderIsSubmitted()
    {
        _orderData = new OrderData(_equipment, _rentalPeriod);
        _order = InitializeAggregate();
    }

    [Given("the rental period is (.*) days from today")]
    public void GivenTheRentalPeriodIsDaysFromToday(int rentalDays)
    {
        _rentalPeriod = new RentalPeriod(DateTime.Today, DateTime.Today.AddDays(rentalDays));
    }

    [Given(@"the approval is requested for order")]
    public void GivenTheApprovalIsRequestedForOrder()
    {
        GivenTheOrderIsSubmitted();
        _order.RequestApproval(new RequestOrderApproval(_order.Id));
    }
    
    [Given(@"the order is approved")]
    public void GivenTheOrderIsApproved()
    {
        GivenTheApprovalIsRequestedForOrder();
        _order.Approve(new ApproveOrder(_order.Id));
    }

    [Given(@"the order is fully paid")]
    public void GivenTheOrderIsFullyPaid()
    {
        GivenTheOrderIsApproved();
        _order.ConfirmPayment(new ConfirmOrderPayment(_order.Id, _order.OrderPayment.TotalMoney));
    }
    
    [Given(@"the order is reserved")]
    public void GivenTheOrderIsReserved()
    {
        GivenTheOrderIsFullyPaid();
        _order.ReserveEquipment(new ReserveEquipment(_order.Id));
    }
    
    [Given(@"the order is in realisation")]
    public void GivenTheOrderIsInRealisation()
    {
        GivenTheOrderIsReserved();
        _order.ConfirmEquipmentRent(new ConfirmEquipmentRent(_order.Id));
    }
    
    [Given(@"there is (.*) equipments of type (.*) which rental price is (.*)")]
    public void GivenThereIsEquipmentsOfTypeWhichRentalPriceIs(int numberOfEquipment, string type, decimal price)
    {
        var equipmentType = new EquipmentType(type, new Money(price));
        for (var i = 0; i < numberOfEquipment; i++)
        {
            _equipment.Add(new EquipmentItem(equipmentType));
        }
    }
    
    [Given("the total equipment price is (.*) per day")]
    public void GivenTheTotalEquipmentPriceIsPerDay(decimal dailyPrice)
    {
        _equipment = new List<EquipmentItem>
        {
            new(new EquipmentType("EQ1", new Money(dailyPrice)))
        };
    }
}