using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v) );

            builder
                .Property(e => e.ConnectionStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new ConnectionStatus(v) );

            builder
                .Property(e => e.AgeClassification)
                .HasConversion(
                    v => v.ToString(),
                    v => new AgeClassification(v));

            builder
                .Property(e => e.Gender)
                .HasConversion(
                    v => v.ToString(),
                    v => new Gender(v));

            builder
                .Property(e => e.MaritalStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new MaritalStatus(v));

            builder
                .Property(e => e.CommunicationPreference)
                .HasConversion(
                    v => v.ToString(),
                    v => new CommunicationType(v));

            // https://stackoverflow.com/questions/49176801/indexes-and-owned-types
            builder.OwnsOne(x => x.FullName, xx =>
            {
                xx.HasIndex(o => o.FirstName);
                xx.HasIndex(o => o.LastName);
            });

            builder.HasIndex(o => o.ConnectionStatus);
        }
    }
}
