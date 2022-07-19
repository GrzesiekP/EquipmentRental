using System;

namespace Orders.Models.ValueObjects
{
    public record RentalPeriod
    {
        public DateTime RentalDate { get; }
        public DateTime ReturnDate { get; }
        
        public RentalPeriod(DateTime rentalDate, DateTime returnDate)
        {
            if (rentalDate > returnDate)
            {
                throw new ArgumentException(
                    $"Rental date {rentalDate.ToShortDateString()} cannot be after return date {returnDate.ToShortDateString()}");
            }
            
            RentalDate = rentalDate;
            ReturnDate = returnDate;
        }

        public TimeSpan Value => ReturnDate - RentalDate;
    }
}