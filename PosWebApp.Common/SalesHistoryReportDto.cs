using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosWebAppCommon
{
    public class SalesHistoryReportDto
    {
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Cashier { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
