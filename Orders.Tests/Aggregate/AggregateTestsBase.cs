using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Events;
using Orders.Aggregate;
using Orders.Models.Entities;
using Orders.Models.ValueObjects;
using Tests.Core;

namespace Orders.Tests.Aggregate;

public class AggregateTestsBase<T> : TestsBase where T: IEvent
{
    protected Guid OrderId { get; private set; }
    protected string? UserEmail { get; private set; }
    protected OrderData? OrderData { get; set; }
    protected Order Order { get; private set; } = new();
    protected int InitialVersion { get; private set; }

    private IEnumerable<IEvent> GetAggregateEvents() => Order.DequeueUncommittedEvents();
    
    protected override void Given()
    {
        var equipmentItems = new List<EquipmentItem>
        {
            new( new EquipmentType("TYPE_1", 15)),
            new(new EquipmentType("TYPE_2", 60))
        };
        OrderData = new OrderData(equipmentItems, new RentalPeriod(DateTime.Today, DateTime.Today.AddDays(3)));
        UserEmail = "user@gmail.com";
        Order = InitializeAggregate();
    }

    private Order InitializeAggregate()
    {
        OrderId = Guid.NewGuid();
        var aggregate = Order.Submit(OrderId, OrderData, UserEmail);

        InitialVersion = aggregate.Version;

        return aggregate;
    }

    protected T? GetEvent()
    {
        var e = GetAggregateEvents().SingleOrDefault(e => e.GetType() == typeof(T));
        return e != null ? (T)e : default;
    }
}