using DemoApi.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configuration
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User).WithMany(x => x.Sessions).HasForeignKey(x => x.UserId);
        }
    }
}
