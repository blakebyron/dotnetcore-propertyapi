using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Property.Infrastructure.Data;

namespace Property.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().SeedData().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // ASP.NET Core 3.0+:
                // The UseServiceProviderFactory call attaches the
                // Autofac provider to the generic hosting mechanism.
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }


    public static class IHostBuilderExtensions
    {
        /// <summary>
        /// Create some sample data
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<PropertyContext>();

                Int32 itemsToAdd = 50;
                for (int i = 1; i <= itemsToAdd; i++)
                {
                    var pr = new Property.Core.PropertyReference($"P{ i.ToString().PadLeft(3,'0') }");
                    var p = Property.Core.Property.CreateWithDescription(pr, $"My Property {i}");
                    context.Properties.Add(p);
                }

                context.SaveChanges();
            }

            return host;
        }
    }
}
