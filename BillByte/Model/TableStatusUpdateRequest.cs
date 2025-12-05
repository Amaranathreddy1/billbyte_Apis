namespace BillByte.Model
{
    public class TableStatusUpdateRequest
    {
        public string TableName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // "Available" / "Occupied" / "Billed"
    }
}
