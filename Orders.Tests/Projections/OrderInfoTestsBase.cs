using System;
using System.Collections.Generic;
using Orders.Projections;
using Orders.ValueObjects;
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
            new("CODE1", 10m)
        };
        OrderData = new OrderData(
            items,
            DateTime.Now,
            DateTime.Now.AddDays(3));
        OrderId = Guid.NewGuid();
        UserEmail = "xd@xd.pl";
    }
}





