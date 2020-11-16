using System;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.Data.EntityConfiguration;

namespace Property.Infrastructure.Data
{
    using Property.Core;
    using System.Configuration;

    public class PropertyContext:DbContext
    {

        public DbSet<Property> Properties { get; set; }

        public PropertyContext()
        {

        }

        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if the options haven't been configuration then default to the following.
            if (!optionsBuilder.IsConfigured)
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string connectionString = "Server=.\\;Database=PropertyDB;User ID=sa;Password=Pass@word;";
                optionsBuilder.UseSqlServer(connectionString);
            }

            ////if the options haven't been configuration then default to the following.
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PropertyTypeConfig());
            //base.OnModelCreating(modelBuilder);
            //Add-Migration InitialCreate -StartupProject Property.Infrastructure -Context PropertyContext -OutputDir "Data/Migrations" -Namespace Data.Migrations
            //remove-migration -startupproject Property.Infrastructure
            //Script-Migration -StartupProject Property.Infrastructure -From 0 -To InitialCreate -Project Property.Infrastructure 
        }
    }
}
