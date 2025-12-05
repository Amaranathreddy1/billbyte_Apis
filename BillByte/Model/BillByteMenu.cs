namespace BillByte.Model
{
    public class BillByteMenu
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemTypeId { get; set; }
        public decimal ItemCost { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ImageUrl { get; set; }
        public string CreatedBy { get; set; }

        //public FoodType FoodType { get; set; } // navigation

        //public string FoodTypeName { get; set; }
}
}
