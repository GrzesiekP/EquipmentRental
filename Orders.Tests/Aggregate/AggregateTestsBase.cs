using System;
using System.Collections.Generic;
using Core.Domain.Events;
using Orders.Aggregate;
using Orders.ValueObjects;

namespace Orders.Tests.Aggregate;

public class AggregateTestsBase : TestsBase
{
    protected Guid OrderId { get; private set; }
    protected OrderData OrderData { get; set; }
    protected Order Order { get; private set; }
    protected int InitialVersion { get; private set; }
    
    protected IEnumerable<IEvent> GetAggregateEvents() => Order.DequeueUncommittedEvents();
    
    protected override void Given()
    {
        OrderData = new OrderData
        {
            Equipment = new List<EquipmentItem>
            {
                new() { EquipmentTypeCode = "TYPE_1", RentalPrice = 15 },
                new() { EquipmentTypeCode = "TYPE_2", RentalPrice = 60 }
            }
        };
        
        Order = InitializeAggregate();
    }

    private Order InitializeAggregate()
    {
        OrderId = Guid.NewGuid();
        var aggregate = Order.Submit(OrderId, OrderData);

        InitialVersion = aggregate.Version;

        return aggregate;
    }
}