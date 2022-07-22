using Core.Domain.Events;
using Orders.Events;
using Orders.Models.ValueObjects;

namespace Orders.Models.Entities
{
    public record OrderPayment
    {
        public Money PaidMoney { get; private set; }
        public Money TotalMoney { get; private set; }

        public OrderPayment(Money totalMoney)
        {
            TotalMoney = totalMoney;
        }

        public void Pay(Money amount) => PaidMoney += amount;
        
        public Money RemainingAmount() => TotalMoney - PaidMoney;
        public bool IsEnoughForFullPayment(Money amount) => (amount + PaidMoney).Amount >= TotalMoney.Amount;
    }
}