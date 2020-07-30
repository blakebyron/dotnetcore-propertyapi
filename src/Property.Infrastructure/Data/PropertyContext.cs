using System;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.Data.EntityConfiguration;

namespace Property.Infrastructure.Data
{
    using Property.Core;

    public class PropertyContext:DbContext
    {

        public DbSet<Property> Properties { get; set; }

        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PropertyTypeConfig());
            //base.OnModelCreating(modelBuilder);
        }
    }
}
