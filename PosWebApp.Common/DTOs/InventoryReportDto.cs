namespace PosWebAppCommon.DTOs
{
    public class InventoryReportDto
    {
        public string ItemName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
