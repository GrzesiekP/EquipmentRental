using System;
using System.Collections.Generic;
using Core.Domain.Events;
using Orders.Aggregate;

namespace Orders.Tests.Aggregate;

public class AggregateTestsBase : TestsBase
{
    protected Guid OrderId { get; private set; }
    protected Order Order { get; private set; }
    protected int InitialVersion { get; private set; }
    
    protected IEnumerable<IEvent> GetAggregateEvents() => Order.DequeueUncommittedEvents();
    
    protected override void Given()
    {
        Order = InitializeAggregate();
    }

    private Order InitializeAggregate()
    {
        OrderId = Guid.NewGuid();
        var aggregate = Order.Submit(OrderId);

        InitialVersion = aggregate.Version;

        return aggregate;
    }
}