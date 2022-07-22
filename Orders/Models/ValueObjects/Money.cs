using System;

namespace Orders.Models.ValueObjects
{
    public record Money
    {
        public Money(decimal amount)
        {
            Amount = amount >= 0 ? amount : throw new ArgumentException("Amount cannot be negative");
        }
        
        public decimal Amount { get; private set; }

        public static Money operator +(Money m1, Money m2) => new Money(m1.Amount + m1.Amount);
        public static Money operator -(Money m1, Money m2) => new Money(m1.Amount - m1.Amount);
        public static Money operator /(Money m1, decimal d) => new Money(m1.Amount / d);
        public static Money operator *(Money m1, decimal d) => new Money(m1.Amount * d);
    }
}