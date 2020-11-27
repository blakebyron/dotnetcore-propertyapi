using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Property.Api.Infrastructure.Mvc
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomMvc(this IServiceCollection services, Assembly assembly)
        {
            services.AddControllers(controllers =>
            {
                //by default asp.net core returns responses in default form in unsupported type is requested
                //enabling return of 406 if unsupported media type requested
                controllers.ReturnHttpNotAcceptable = true;
                controllers.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                //default behaviour for binding complex types is to assume they come from request body
                //Enabling this setting allows mapping of MediatR request objections in the controller action
                options.SuppressInferBindingSourcesForParameters = true;
            })
            // Added for functional tests
            .AddApplicationPart(assembly);

        }
    }
}
