using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;
using Property.Api.Infrastructure.Hateoas;

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
                controllers.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                controllers.OutputFormatters.Insert(0,new PropertyResourceFormatter());
            })
            //JsonPatchDocument still relies on NewtonsoftJson so we need to configure this
            .AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
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

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            //Logic taken from
            //https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
