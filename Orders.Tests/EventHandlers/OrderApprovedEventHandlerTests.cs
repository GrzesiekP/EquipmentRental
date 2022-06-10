using System;
using System.Threading;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orders.Commands;
using Orders.EventHandlers;
using Orders.Events;

namespace Orders.Tests.EventHandlers;

[TestClass]
public class OrderApprovedEventHandlerTests : TestsBase
{
    private OrderApprovedEventHandler _handler;
    private readonly Mock<IMediator> _mediatorMock = new();
    private Guid _orderId;
    
    protected override void Given()
    {
        base.Given();
        
        _orderId = Guid.NewGuid();
        _handler = new OrderApprovedEventHandler(_mediatorMock.Object);
    }

    protected override void When()
    {
        base.When();

        _handler.Handle(new OrderApproved(_orderId), new CancellationToken());
    }

    [TestMethod]
    public void ThenRequestOrderApprovalIsPublished()
    {
        _mediatorMock.Verify(m => m.Send(
            It.Is<NotifyClient>(
                r => r.OrderId == _orderId), It.IsAny<CancellationToken>()), 
            Times.Once);
    }

}