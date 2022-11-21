namespace Equipment.Models.ValueObjects
{
    public record Money
    {
        public Money(decimal amount)
        {
            Amount = amount >= 0 ? amount : throw new ArgumentException("Amount cannot be negative");
        }
        public static implicit operator Money(decimal amount)
        {
            return new Money(amount);
        }
        
        public decimal Amount { get; private set; }

        public static Money operator +(Money m1, Money m2) => new Money(m1.Amount + m2.Amount);
        public static Money operator -(Money m1, Money m2) => new Money(m1.Amount - m2.Amount);
        public static Money operator /(Money m1, decimal d) => new Money(m1.Amount / d);
        public static Money operator *(Money m1, decimal d) => new Money(m1.Amount * d);
    }
}