using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using af = Autofac.Module;

namespace Property.Api.Infrastructure.EntityFramework
{
    public class EntityFrameworkModule: af
    {
        private readonly bool IsInMemoryDatabaseEnabled;
        private readonly string ConnectionString;


        public EntityFrameworkModule(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            };
            this.IsInMemoryDatabaseEnabled = configuration.GetValue<bool>("PropertyApiSettings:IsInMemoryDataseEnabled");
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
    }
}
