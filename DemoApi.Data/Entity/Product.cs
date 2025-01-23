namespace DemoApi.Data.Entity
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
