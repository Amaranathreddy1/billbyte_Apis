using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillByte.Model
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int TableOrderId { get; set; }
        [ForeignKey("TableOrderId")]
        public TableOrder? TableOrder { get; set; }

        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal ItemCost { get; set; }
        public int Qty { get; set; }
        public string? ImageUrl { get; set; }
    }
}
