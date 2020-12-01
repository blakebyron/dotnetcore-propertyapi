using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Property.Api.IntegrationTests.Features.PropertyCollection
{
    public class PropertyCollectionScenarioTests : PropertyScenarioBase
    {

        public const string CreateProperty = "api/propertycollection/";

        private readonly PropertyCollectionBuilder _propertyCollectionBuilder = new PropertyCollectionBuilder();

        [Fact]
        public async Task create_multiple_properties_and_response_created_status_code()
        {
            var propertyList = _propertyCollectionBuilder
                                   .WithTestValues()
                                   .WithTestValues()
                                   .Build();

            string jsonContenxt = JsonSerializer.Serialize(propertyList);

            using (var server = CreateServer())
            {
                var content = new StringContent(jsonContenxt, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(CreateProperty, content);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }
    }
}
