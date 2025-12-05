namespace BillByte.DTO
{
    public class SalesPointDto
    {
        public DateTime DayDate { get; set; }
        public string DayName { get; set; } = "";
        public int OrderInCount { get; set; }
        public int DeliveryCount { get; set; }
        public int ParcelCount { get; set; }
    }
}
