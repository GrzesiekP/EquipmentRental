using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Core.Domain.Events;
using FluentAssertions;
using Orders.Aggregate;
using Orders.Aggregate.ValueObjects;
using Orders.Commands;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Orders.BehaviourTests.Steps;

[Binding]
public sealed class OrderStepsDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private string _userEmail = "user@example.com";
    private RentalPeriod _rentalPeriod = new(DateTime.Today, DateTime.Today.AddDays(1));
    private List<EquipmentItem> _equipment = new()
    {
        new EquipmentItem(new EquipmentType("NEVIS", new Money(20)))
    };
    private OrderData _orderData;
    private Order _order;

    // private IEnumerable<IEvent> GetAggregateEvents() => Order.DequeueUncommittedEvents();

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
        _order.ReserveEquipment(new ReserveEquipment());
    }
    
    [When("returning equipment")]
    public void WhenReturningEquipment()
    {
        _order.ReturnEquipment(new ReturnEquipment());
    }

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
    
    private Order InitializeAggregate()
    {
        var orderId  = Guid.NewGuid();
        var aggregate = Order.Submit(orderId, _orderData, _userEmail);

        // InitialVersion = aggregate.Version;

        return aggregate;
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
    public void GivenTheAprrovalIsRequested()
    {
        _order.RequestApproval(new RequestOrderApproval(_order.Id));
    }
    
    [Given(@"the order is approved")]
    public void GivenTheOrderIsApproved()
    {
        GivenTheAprrovalIsRequested();
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

    [Then(@"order equipment is reserved for order reservation period")]
    public void ThenOrderEquipmentIsReservedForOrderReservationPeriod()
    {
        _order.OrderData.EquipmentItems.Should().Satisfy(e => e.IsReservedFor(_rentalPeriod));
    }

    [Then(@"equipment is (.*)")]
    public void ThenEquipmentIsRent(EquipmentStatus equipmentStatus)
    {
        _order.OrderData.EquipmentItems.Should().Satisfy(e => e.Status == equipmentStatus);
    }
}