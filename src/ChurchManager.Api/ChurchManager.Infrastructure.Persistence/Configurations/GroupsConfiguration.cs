using ChurchManager.Domain;
using ChurchManager.Persistence.Models.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class GroupsConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v));

            // GroupFeatures (Many-to-Many)
            builder
                .HasMany(p => p.Features)
                .WithMany(p => p.Groups)
                .UsingEntity(j => j.ToTable("GroupFeatures", "Groups"));
        }
    }
}