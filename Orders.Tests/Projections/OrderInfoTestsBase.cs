using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Aggregate.ValueObjects;
using Orders.Events;
using Orders.Projections;
using Orders.ValueObjects;

namespace Orders.Tests.Projections;

public class OrderInfoTestsBase : TestsBase
{
    protected OrderInfo Projection;

    protected Guid OrderId;
    protected OrderData OrderData;
    protected string ClientEmail;
    
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
        ClientEmail = "xd@xd.pl";
    }
}





