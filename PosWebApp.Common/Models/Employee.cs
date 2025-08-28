namespace PosWebAppCommon.Models
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
    }
}
