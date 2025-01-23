namespace DemoApi.Model.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
