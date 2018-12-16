using Raveshmand.Localization.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raveshmand.Localization.EntityFramework
{
    public class LocalizationRecordConfiguration : IEntityTypeConfiguration<LocalizationRecord>
    {
        public void Configure(EntityTypeBuilder<LocalizationRecord> builder)
        {
            builder.ToTable("LocalizationRecord", "dbo");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(256).IsRequired();
            builder.Property(a => a.Value).IsRequired();
            builder.Property(a => a.ResourceName).HasMaxLength(256).IsRequired();
            builder.Property(a => a.CultureName).HasMaxLength(10).IsRequired();

        }
    }
}