// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
using System;
using System.Collections.Generic;

namespace EquipmentRental.WebApi.Models
{
    public class SubmitOrderInput
    {
        public List<EquipmentItem> EquipmentItems { get; set; }
        public DateTime RentalDate { get; set;  }
        public DateTime ReturnDate { get; set;  }
    }
}