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
            return CreateServer(true);
        }

        public TestServer CreateServer(bool IsPropertyCollectionEnabled)
        {
            var settings = new Dictionary<string, string>
            {
                {"PropertyApiSettings:IsInMemoryDataseEnabled", "true"},
                {"PropertyApiSettings:IsInMemoryDummyDataRequired", "true"},
                {"FeatureManagement:IsPropertyCollectionResourceEnabled", IsPropertyCollectionEnabled.ToString()},
            };
            return CreateServer(settings);
        }

        public TestServer CreateServer(Dictionary<string, string> settings)
        {
            var server = new TestServer(new WebHostBuilder()
            .ConfigureServices(service => service.AddAutofac())
            .ConfigureAppConfiguration(cd =>
            {
                cd.AddInMemoryCollection(settings)
                .AddEnvironmentVariables();
            })
            .UseStartup<Startup>());

            return server;
        }
    }
}
