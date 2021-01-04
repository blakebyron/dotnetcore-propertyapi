using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Property.Api.IntegrationTests.Features.PropertyCollection
{
    public class PropertyCollectionScenarioTests : PropertyScenarioBase
    {

        public const string CreateProperty = "api/propertycollection/";

        private readonly PropertyCollectionBuilder _propertyCollectionBuilder = new PropertyCollectionBuilder();
        private readonly Dictionary<String, String> settings;
        private readonly string IsPropertyCollectionSettingName = "FeatureManagement:IsPropertyCollectionResourceEnabled";

        public PropertyCollectionScenarioTests()
        {
            this.settings = new Dictionary<string, string>
            {
                {"PropertyApiSettings:IsInMemoryDataseEnabled", "true"},
                {"PropertyApiSettings:IsInMemoryDummyDataRequired", "true"},
                {IsPropertyCollectionSettingName, true.ToString()},
            };
        }
        [Fact]
        public async Task create_multiple_properties_and_response_created_status_code()
        {
            var propertyList = _propertyCollectionBuilder
                                   .WithTestValues()
                                   .WithTestValues()
                                   .Build();

            string jsonContenxt = JsonSerializer.Serialize(propertyList);

            using (var server = CreateServer(settings))
            {
                var content = new StringContent(jsonContenxt, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(CreateProperty, content);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Fact]
        //relies on test data being created when correct parameters are passed to
        public async Task enabled_get_multiple_properties_and_response_ok_status()
        {
            this.settings[IsPropertyCollectionSettingName] = true.ToString();
            using (var server = CreateServer(settings))
            {
                var response = await server.CreateClient()
                    .GetAsync($"api/propertycollection/(P010,P011)");

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        //relies on test data being created when correct parameters are passed to
        public async Task disabled_resource_get_multiple_properties_and_response_ok_status()
        {
            this.settings[IsPropertyCollectionSettingName] = false.ToString();
            using (var server = CreateServer(settings))
            {
                var response = await server.CreateClient()
                    .GetAsync($"api/propertycollection/(P010,P011)");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
