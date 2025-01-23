namespace DemoApi.Common.Enums
{
    public static class Enums
    {
        public enum Roles : int
        {
            [StringValue("Administrator")]
            Administrator = 1,
            [StringValue("Manager")]
            Manager,
            [StringValue("Customer")]
            Customer,
            [StringValue("Employee")]
            Employee
        }
    }
}
