namespace DemoApi.Data.Entity
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool Valid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public virtual User User { get; set; }
    }
}
