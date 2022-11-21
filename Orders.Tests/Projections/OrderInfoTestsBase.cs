using System;
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
        
        var equipmentItems = new Dictionary<string, int>
        {
            { "TYPE_1", 1 },
            { "TYPE_2", 1 },
        };
        OrderData = new OrderData(
            equipmentItems,
            new RentalPeriod(DateTime.Now, DateTime.Now.AddDays(3)),
            60);
        OrderId = Guid.NewGuid();
        UserEmail = "xd@xd.pl";
    }
}





