using System;
using Microsoft.EntityFrameworkCore;

namespace Property.Infrastructure.Data.EntityConfiguration
{
    using Property.Core;

    public class PropertyTypeConfig : IEntityTypeConfiguration<Property>
    {
        const Int32 PropertyReferenceMaxFieldLength = 250;
        public PropertyTypeConfig()
        {
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("tbl_Property");
            builder.HasKey(x => x.ID)
                .HasName("PropertyID");
            builder.OwnsOne(x => x.Reference)
                .Property(x => x.Reference)
                .HasColumnName("PropertyReference")
                .HasMaxLength(PropertyReferenceMaxFieldLength)
                .IsRequired();

        }
    }
}
