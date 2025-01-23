using DemoApi.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.UserName).IsUnique();
            builder.HasIndex(t => t.Email).IsUnique();

        }
    }
}
