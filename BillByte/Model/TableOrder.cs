using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BillByte.Model
{
    public class TableOrder
    {
        [Key]
        public int Id { get; set; }
        public string TableNumber { get; set; } = "";
        public string? OrderType { get; set; }    // OrderIn / Delivery / Parcel
        public DateTime? StartTime { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }       // Available, Occupied, Billed
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

}

