namespace PosWebAppCommon.Models
{
    public class Item : BaseEntity
    {
        public string ItemName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public int ReorderLevel { get; set; } = 0;
        public decimal SellingPrice { get; set; } = 0.00m;
        public decimal CostPrice { get; set; } = 0.00m;
        public bool IsActive { get; set; } = true;
    }
}