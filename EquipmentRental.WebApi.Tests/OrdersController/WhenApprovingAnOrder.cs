using System;
using System.Net;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orders.Commands;

namespace EquipmentRental.WebApi.Tests.OrdersController;

[TestClass]
public class WhenApprovingAnOrder : OrdersControllerTestsBase
{
    private Guid _orderId;

    protected override void Given()
    {
        base.Given();
        
        _orderId = Guid.NewGuid();
    }

    protected override void When()
    {
        base.When();
        
        Result = Controller!.ApproveRequest(_orderId).Result;
    }

    [TestMethod]
    public void ThenResponseIsSuccessful()
    {
        AcceptedResult.Should()
            .NotBeNull();
        AcceptedResult!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
    }
    
    [TestMethod]
    public void ThenApproveOrderMessageIsPublished()
    {
        MediatorMock!.Verify(m => m.Send(
            It.Is<ApproveOrder>(x => x.OrderId == _orderId), 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}