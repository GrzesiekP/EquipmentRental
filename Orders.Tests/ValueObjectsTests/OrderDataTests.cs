using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;

namespace Orders.Tests.ValueObjectsTests;

[TestClass]
public class OrderDataTests
{
    private OrderData? _orderData;
    private const int ExpectedRentalDays = 3;

    [TestInitialize]
    public void Initialize()
    {
        var rentalDate = new DateTime(2030, 01, 01);
        var returnDate = rentalDate.AddDays(ExpectedRentalDays);

        var items = new List<EquipmentItem>
        {
            new(new EquipmentType("CODE1", new Money(10m))),
            new(new EquipmentType("CODE2", new Money(90m)))
        };
        
        _orderData = new OrderData(
            items,
            new RentalPeriod(rentalDate, returnDate)
        );
    }
    
    [TestMethod]
    public void CalculateRentalDaysTests()
    {
        var expectedPrice = _orderData.EquipmentItems.Sum(i => i.Type.RentalPrice.Amount) * ExpectedRentalDays;
        Assert.AreEqual(expectedPrice, _orderData.CalculateTotalPrice().Amount);
    }
}