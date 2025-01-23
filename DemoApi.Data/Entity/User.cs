using Microsoft.AspNetCore.Identity;

namespace DemoApi.Data.Entity
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsStatus { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual List<Session> Sessions { get; set; }
    }
}
