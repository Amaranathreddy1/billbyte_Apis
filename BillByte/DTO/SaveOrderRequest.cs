namespace BillByte.DTO
{
    public class SaveOrderRequest
    {
        public string TableNumber { get; set; }
        public string ZoneType { get; set; }
        public int UserId { get; set; }
        public string ItemIds { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime StartTime { get; set; }
        public string UserType { get; set; }
        public string PaymentMode { get; set; }
        public string Status { get; set; }
        public List<TableOrderItemDto> Items { get; set; }
    }
}
