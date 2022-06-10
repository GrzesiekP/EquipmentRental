using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.ValueObjects;

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
            new("CODE1", 10m),
            new("CODE2", 90m),
        };
        
        _orderData = new OrderData(
            items,
            rentalDate,
            returnDate
        );
    }
    
    [TestMethod]
    public void CalculateRentalDaysTests()
    {
        var expectedPrice = _orderData.EquipmentItems.Sum(i => i.RentalPrice) * ExpectedRentalDays;
        Assert.AreEqual(expectedPrice, _orderData.CalculateTotalPrice());
    }
}