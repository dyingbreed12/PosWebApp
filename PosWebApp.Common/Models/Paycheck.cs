namespace PosWebAppCommon.Models
{
    public class Paycheck : BaseEntity
    {
        public int EmployeeId { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public DateTime PayDate { get; set; }
    }
}
