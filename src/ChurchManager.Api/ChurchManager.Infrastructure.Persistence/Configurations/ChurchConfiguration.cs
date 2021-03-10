using ChurchManager.Domain;
using ChurchManager.Persistence.Models.Churches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class ChurchConfiguration : IEntityTypeConfiguration<Church>
    {
        public void Configure(EntityTypeBuilder<Church> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v));
        }
    }
}