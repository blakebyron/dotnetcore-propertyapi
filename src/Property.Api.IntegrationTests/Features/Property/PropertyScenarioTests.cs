using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Property.Api.Features.Property;
using Xunit;

namespace Property.Api.IntegrationTests.Features.Property
{
    public class PropertyScenarioTests: PropertyScenarioBase
    {

        public const string GetProperties = "api/properties";
        public const string CreateProperty = "api/properties/";

        private readonly PropertyBuilder _propertyBuilder = new PropertyBuilder();

        [Fact]
        public async Task Get_all_properties_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(GetProperties);
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task create_individual_property_and_response_created_status_code()
        {
            var property = _propertyBuilder
                                .WithTestValues()
                                .Build();

            string jsonContenxt = JsonSerializer.Serialize(property); 

            using (var server = CreateServer())
            {
                var content = new StringContent(jsonContenxt, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(CreateProperty, content);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Fact]
        public async Task create_individual_property_and_response_badrequest_status_code()
        {
            var property = _propertyBuilder
                                .WithTestValues()
                                .Reference("asdasd")
                                .Build();

            string jsonContenxt = JsonSerializer.Serialize(property);

            using (var server = CreateServer())
            {
                var content = new StringContent(jsonContenxt, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(CreateProperty, content);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }


        [Fact]
        public async Task update_individual_property_description_success()
        {
            var property = _propertyBuilder
                    .WithTestValues()
                    .Build();

            string jsonContenxt = JsonSerializer.Serialize(property);

            using (var server = CreateServer())
            {
                var content = new StringContent(jsonContenxt, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(CreateProperty, content);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);


                var patchDoc = new JsonPatchDocument<PropertyPatchModel>();
                patchDoc.Add(x => x.PropertyDescription, "The test new description");

                string patchJsonContenxt = JsonSerializer.Serialize(patchDoc.Operations);

                var patchContent = new StringContent(patchJsonContenxt, UTF8Encoding.UTF8, "application/json");

                var patchResponse = await server.CreateClient()
                    .PatchAsync(response.Headers.Location, patchContent);
                Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

            }
        }
    }
}