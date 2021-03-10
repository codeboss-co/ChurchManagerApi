using ChurchManager.Domain;
using ChurchManager.Persistence.Models.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class FamilyConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v));
        }
    }
}