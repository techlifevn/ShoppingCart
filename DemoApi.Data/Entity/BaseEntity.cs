namespace DemoApi.Data.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? UserId { get; set; }
    }
}
