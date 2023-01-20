using Core.Models;

namespace Orders.Models.Entities
{
    public record OrderPayment(Money TotalMoney)
    {
        public Money PaidMoney { get; private set; } = 0;
        public Money TotalMoney { get; } = TotalMoney;

        public void Pay(Money amount) => PaidMoney += amount;
        
        public bool IsEnoughForFullPayment(Money amount) => (amount + PaidMoney).Amount >= TotalMoney.Amount;
    }
}