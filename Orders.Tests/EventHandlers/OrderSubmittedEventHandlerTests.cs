using System;
using System.Collections.Generic;
using System.Threading;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orders.Commands;
using Orders.EventHandlers;
using Orders.Events;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;
using Tests.Core;

namespace Orders.Tests.EventHandlers;

[TestClass]
public class OrderSubmittedEventHandlerTests : TestsBase
{
    private OrderSubmittedEventHandler? _handler;
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

        _handler!.Handle(GenerateTestEvent(), new CancellationToken());
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
        var equipmentItems = new Dictionary<string, int>
        {
            { "TYPE_1", 1 },
            { "TYPE_2", 1 },
        };
        var orderData = new OrderData(
            equipmentItems,
            new RentalPeriod(DateTime.Now, DateTime.Now.AddDays(3)),
            60);
        return new OrderSubmitted(
            _orderId,
            orderData,
            "xd@xd.pl"
            );
    }
}