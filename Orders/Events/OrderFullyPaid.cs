using Core.Domain.Events;
using Core.Models;

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