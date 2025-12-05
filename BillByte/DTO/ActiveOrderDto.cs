namespace BillByte.DTO
{
    public class ActiveOrderDto
    {
        public int TableOrderId { get; set; }
        public string TableNumber { get; set; }
        public string ZoneType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime StartTime { get; set; }
        public string UserType { get; set; }
        public List<TableOrderItemDto> Items { get; set; }
    }

}
