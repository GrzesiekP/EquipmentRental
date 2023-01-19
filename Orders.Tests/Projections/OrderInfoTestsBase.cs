using System;
using System.Collections.Generic;
using Core.Models;
using Equipment.Models.Entities;
using Equipment.Models.ValueObjects;
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
        
        var equipmentItems = new List<EquipmentItem>
        {
            new(new EquipmentType("CT NEVIS", 20)),
            new(new EquipmentType("CT NUPTUSE", 20)),
        };
        OrderData = new OrderData(
            equipmentItems,
            new RentalPeriod(DateTime.Now, DateTime.Now.AddDays(3)));
        OrderId = Guid.NewGuid();
        UserEmail = "xd@xd.pl";
    }
}





