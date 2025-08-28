namespace PosWebAppCommon.DTOs;

public class DashboardSummaryDto
{
    public decimal TotalSales { get; set; }
    public decimal TotalProfit { get; set; }
    public int TotalTransactions { get; set; }
    public int LowStockItems { get; set; }
    public List<DailySalesDto> DailySales { get; set; } = new List<DailySalesDto>();
    public List<TopSellingItemDto> TopSellingItems { get; set; } = new List<TopSellingItemDto>();
}