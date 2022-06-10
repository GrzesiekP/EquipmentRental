using System;
using System.Collections.Generic;
using System.Threading;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orders.Commands;
using Orders.EventHandlers;
using Orders.Events;
using Orders.ValueObjects;

namespace Orders.Tests.EventHandlers;

[TestClass]
public class OrderSubmittedEventHandlerTests : TestsBase
{
    private OrderSubmittedEventHandler _handler;
    private readonly Mock<IMediator> _mediatorMock = new();
    private Guid _orderId;
    
    protected override void Given()
    {
        base.Given();
        
        _orderId = Guid.NewGuid();
        _handler = new OrderSubmittedEventHandler(_mediatorMock.Object);
    }

    protected override void When()
    {
        base.When();

        _handler.Handle(GenerateTestEvent(), new CancellationToken());
    }

    [TestMethod]
    public void ThenRequestOrderApprovalIsPublished()
    {
        _mediatorMock.Verify(m => m.Send(
            It.Is<RequestOrderApproval>(
                r => r.OrderId == _orderId), It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    private OrderSubmitted GenerateTestEvent()
    {
        var items = new List<EquipmentItem>
        {
            new("CODE1", 10m)
        };
        var orderData = new OrderData(
            items,
            DateTime.Now,
            DateTime.Now.AddDays(3));
        return new OrderSubmitted(
            _orderId,
            orderData,
            "xd@xd.pl"
            );
    }
}