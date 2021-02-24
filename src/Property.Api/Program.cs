using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Property.Core.ValueObjects;
using Property.Infrastructure.Data;
// Additional required namespaces
using Serilog;

namespace Property.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static void Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();

            CreateHostBuilder(args).Build().SeedData().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // ASP.NET Core 3.0+:
                // The UseServiceProviderFactory call attaches the
                // Autofac provider to the generic hosting mechanism.
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static Serilog.ILogger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                //.MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                //.Enrich.FromLogContext()
                .WriteTo.File("log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

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

                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }

                Int32 itemsToAdd = 50;
                for (int i = 1; i <= itemsToAdd; i++)
                {
                    var pr = new PropertyReference($"P{ i.ToString().PadLeft(3,'0') }");
                    var p = Property.Core.Property.CreateWithDescription(pr, $"My Property {i}");
                    context.Properties.Add(p);
                }

                context.SaveChanges();
            }

            return host;
        }
    }
}
