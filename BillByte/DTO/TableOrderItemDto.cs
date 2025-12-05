namespace BillByte.DTO
{
    public class TableOrderItemDto
    {
        public int TableOrderId { get; set; }
        public string TableNumber { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemCost { get; set; }
        public string ImageUrl { get; set; }
        public int Qty { get; set; }

    }
}
