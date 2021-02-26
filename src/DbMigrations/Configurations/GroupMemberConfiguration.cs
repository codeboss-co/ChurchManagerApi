using Groups.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Kernel;

namespace DbMigrations.Configurations
{
    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v) );

            builder
                .Property(e => e.CommunicationPreference)
                .HasConversion(
                    v => v.ToString(),
                    v => new CommunicationType(v));

        }
    }
}
