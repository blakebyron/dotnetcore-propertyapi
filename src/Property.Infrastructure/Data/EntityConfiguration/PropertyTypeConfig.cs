using System;
using Microsoft.EntityFrameworkCore;

namespace Property.Infrastructure.Data.EntityConfiguration
{
    using Property.Core;

    public class PropertyTypeConfig : IEntityTypeConfiguration<Property>
    {
        public PropertyTypeConfig()
        {
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(x => x.Description);
            builder.OwnsOne(x => x.Reference)
                .Property(x => x.Reference)
                .HasColumnName("PropertyReference");
        }
    }
}
