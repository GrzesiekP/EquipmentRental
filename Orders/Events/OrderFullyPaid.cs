using Core.Domain.Events;
using Orders.Models.ValueObjects;

namespace Orders.Events
{
    public class OrderFullyPaid : IEvent
    {
        public Money Amount { get; private set; }
        
        public OrderFullyPaid(Money amount)
        {
            Amount = amount;
        }
    }
}