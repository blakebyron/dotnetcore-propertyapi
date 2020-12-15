using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using af = Autofac.Module;
using System.Collections.Generic;

namespace Property.Api.Infrastructure.EntityFramework
{
    public class EntityFrameworkModule : af
    {
        private readonly bool IsInMemoryDatabaseEnabled;
        private readonly bool IsInMemoryDummyDataRequired;
        private readonly string ConnectionString;

        public EntityFrameworkModule(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            };
            this.IsInMemoryDatabaseEnabled = configuration.GetValue<bool>("PropertyApiSettings:IsInMemoryDataseEnabled");
            this.IsInMemoryDummyDataRequired = configuration.GetValue<bool>("PropertyApiSettings:IsInMemoryDummyDataRequired");
            this.ConnectionString = configuration.GetConnectionString("ConnectionString");

            if (!this.IsInMemoryDatabaseEnabled && String.IsNullOrWhiteSpace(this.ConnectionString))
            {
                throw new ArgumentNullException("ConnectionString");
            }
        }
        protected override void Load(ContainerBuilder builder)
        {

            var optionsBuilder = new DbContextOptionsBuilder<PropertyContext>();

            if (IsInMemoryDatabaseEnabled)
            {
                optionsBuilder.UseInMemoryDatabase("PropertyDb");
                if (IsInMemoryDummyDataRequired)
                {
                    AddDummyData(optionsBuilder.Options);
                }
            }
            else
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }

            builder.Register<PropertyContext>(ctx =>
            {
                return new PropertyContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            base.Load(builder);
        }

        protected void AddDummyData(DbContextOptions<PropertyContext> contextOptions)
        {
            var ctx = new PropertyContext(contextOptions);
            ctx.Properties.AddRangeAsync(GetDummyDataItems());
            ctx.SaveChanges();
        }

        private IEnumerable<Core.Property> GetDummyDataItems()
        {
            return new List<Core.Property>()
            {
                Core.Property.CreateWithDescription(new Core.PropertyReference("P010"),"Description for Property P010"),
                Core.Property.CreateWithDescription(new Core.PropertyReference("P011"),"Description for Property P011"),
                Core.Property.CreateWithDescription(new Core.PropertyReference("P012"),"Description for Property P012"),
                Core.Property.CreateWithDescription(new Core.PropertyReference("P013"),"Description for Property P013"),
                Core.Property.CreateWithDescription(new Core.PropertyReference("P014"),"Description for Property P014")
            };
        }
    }
}
