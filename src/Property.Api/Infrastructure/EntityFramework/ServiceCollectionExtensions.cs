using System;
using Microsoft.Extensions.DependencyInjection;

namespace Property.Api.Infrastructure.EntityFramework
{
    using Microsoft.Extensions.Configuration;
    using Property.Infrastructure.Data;
    using EntityFramework;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, Assembly assembley)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            };
            if (assembley == null)
            {
                throw new ArgumentNullException(nameof(assembley));
            };

            //services.AddEntityFrameworkInMemoryDatabase()
            //    .AddDbContext<PropertyContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("PropertyDb");
            //    },
            //    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            //);
            services
                .AddDbContext<PropertyContext>(options =>
                {
                    string tConnectionString = configuration.GetConnectionString("ConnectionString");
                    options.UseSqlServer(tConnectionString);
                },
                ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );

            return services;
        }
    }
}
