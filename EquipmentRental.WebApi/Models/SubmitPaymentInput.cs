using System;

namespace EquipmentRental.WebApi.Models
{
    public class SubmitPaymentInput
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}