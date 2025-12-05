namespace BillByte.DTO
{
    public class CreateTableOrderDto
    {
        public string TableNumber { get; set; }   // T2 or D/P
        public string ZoneType { get; set; }     // NonAC / AC / DP
        public int UserId { get; set; }          // later from JWT
        public string ItemIds { get; set; }      // "1:2,5:1"
        public decimal TotalCost { get; set; }
        public DateTime StartTime { get; set; }  // from UI when OrderIn clicked
        public string PaymentMode { get; set; }  // Cash / Card / QR
        public string UserType { get; set; }     // OrderIn / Delivery / Parcel

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public List<CreateTableOrderItemDto> Items { get; set; }
        public string Status { get; set; }
    }
}
