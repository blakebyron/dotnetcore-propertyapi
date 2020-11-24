using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Property.Api.IntegrationTests
{
    public class PropertyScenarioBase
    {
        public TestServer CreateServer()
        {
            var server = new TestServer(new WebHostBuilder()
            .ConfigureServices(service => service.AddAutofac())
            .ConfigureAppConfiguration(cd =>
            {
                cd.AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables();
            })
            .UseStartup<Startup>());

            return server;
        }
    }
}
