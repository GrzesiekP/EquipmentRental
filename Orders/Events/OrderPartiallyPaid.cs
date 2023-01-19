using Core.Domain.Events;
using Core.Models;

namespace Orders.Events
{
    public class OrderPartiallyPaid : IEvent
    {
        public Money Amount { get; private set; }
        
        public OrderPartiallyPaid(Money amount)
        {
            Amount = amount;
        }
    }
}