using System;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orders.Commands;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

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
        Assert.IsNotNull(AcceptedResult);
        Assert.AreEqual((int)HttpStatusCode.Accepted, AcceptedResult.StatusCode);
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