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

    [Given("the rental period is (.*) days from today")]
    public void GivenTheRentalPeriodIs(int rentalDays)
    {
        _rentalPeriod = new RentalPeriod(DateTime.Today, DateTime.Today.AddDays(rentalDays));
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

    [Given(@"there is (.*) equipments of type (.*)")]
    public void GivenThereIsEquipmentsOfType(int numberOfItems, string itemType)
    {
        _equipment = new Dictionary<string, int>()
        {
            { itemType, numberOfItems }
        };
    }
}