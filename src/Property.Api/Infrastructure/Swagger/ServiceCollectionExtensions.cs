using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Property.Api.Infrastructure.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Property Api",
                    Version = "v1"
                });

                setupAction.DocumentFilter<HealthCheckEndpointDocumentFilter>();

                //Find the xml file generated at build time containing endpoint documentaton
                //load it to display information in swagger ui
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                var fi = new FileInfo(xmlCommentsPath);
                if (fi.Exists)
                {
                    setupAction.IncludeXmlComments(xmlCommentsPath);
                }
            });
            return services;

        }
    }
}
