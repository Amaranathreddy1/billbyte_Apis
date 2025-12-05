namespace BillByte.DTO
{
    public class TableOrderDto
    {
        public int TableOrderId { get; set; }
        public string TableName { get; set; }
        public List<TableOrderItemDto> Items { get; set; }
        public string TableNumber { get; set; } = "";
        public string? OrderType { get; set; }
        public DateTime? StartTime { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }

        public string ZoneType { get; set; }
        public int UserId { get; set; }
        public string ItemIds { get; set; }
        public decimal TotalCost { get; set; }
        public string UserType { get; set; }
        public string PaymentMode { get; set; }
    }
}
