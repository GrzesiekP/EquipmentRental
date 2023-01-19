using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using EquipmentRental.WebApi.Models;
using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orders.Commands;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EquipmentRental.WebApi.Tests.OrdersController;

[TestClass]
public class WhenSubmittingAnOrder : OrdersControllerTestsBase
{
    private SubmitOrderInput? _input;
    private DateTime _orderDate;
    private const int OrderDays = 3;

    private SubmitOrder? _sendMessage;

    protected override void Given()
    {
        base.Given();

        MediatorMock!.Setup(m => m.Send(
                It.IsAny<SubmitOrder>(), It.IsAny<CancellationToken>()))
            .Callback((IRequest<Unit> c, CancellationToken _) => _sendMessage = (SubmitOrder)c);
        
        _orderDate = DateTime.Now;
        _input = new SubmitOrderInput
        {
            EquipmentItems = new List<EquipmentItem>
            {
                new() { RentalPrice = 20, EquipmentTypeCode = "NEVIS1" }
            },
            RentalDate = _orderDate,
            ReturnDate = _orderDate.AddDays(OrderDays)
        };
    }

    protected override void When()
    {
        base.When();
        
        Result = Controller!.SubmitOrder(_input).Result;
    }

    [TestMethod]
    public void ThenResponseIsSuccessful()
    {
        Assert.IsNotNull(OkResult);
        Assert.AreEqual((int)HttpStatusCode.OK, OkResult.StatusCode);
    }
    
    [TestMethod]
    public void ThenResponseContainsOrderId()
    {
        var response = OkResult?.Value;
        response.Should()
            .NotBeNull().And
            .BeOfType<Guid>().And
            .NotBe(Guid.Empty);
    }
    
    [TestMethod]
    public void ThenSubmitOrderMessageIsPublished()
    {
        MediatorMock!.Verify(m => m.Send(
            It.IsAny<SubmitOrder>(), 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [TestMethod]
    public void DataInSentSubmitOrderIsCorrect()
    {
        Assert.AreEqual(UserEmail, _sendMessage!.ClientEmail);
        Assert.IsNotNull(_sendMessage.OrderData);
        var orderData = _sendMessage.OrderData;
        Assert.AreEqual(_orderDate, orderData.RentalPeriod.RentalDate);
        Assert.AreEqual(_orderDate.AddDays(OrderDays), orderData.RentalPeriod.ReturnDate);
        Assert.AreEqual(_input!.EquipmentItems.Count, orderData.EquipmentItems.Count);
    }
}