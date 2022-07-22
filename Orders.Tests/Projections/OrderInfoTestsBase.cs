﻿using System;
using System.Collections.Generic;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;
using Orders.Projections;
using Tests.Core;

namespace Orders.Tests.Projections;

public class OrderInfoTestsBase : TestsBase
{
    protected OrderInfo? Projection;

    protected Guid OrderId;
    protected OrderData? OrderData;
    protected string? UserEmail;
    
    protected override void Given()
    {
        base.Given();

        Projection = new OrderInfo();
        
        var items = new List<EquipmentItem>
        {
            new(new EquipmentType("CODE1", new Money(10m)))
        };
        OrderData = new OrderData(
            items,
            new RentalPeriod(DateTime.Now, DateTime.Now.AddDays(3)));
        OrderId = Guid.NewGuid();
        UserEmail = "xd@xd.pl";
    }
}





