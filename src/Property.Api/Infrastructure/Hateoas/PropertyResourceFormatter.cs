using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Property.Api.Infrastructure.Hateoas
{
    public class PropertyResourceFormatter : OutputFormatter
    {
        const string MediatTypeName = "application/vnd.blake.property.hateoas+json";
        const string GetMethodName = "GET";
        const string SelfName = "self";

        public PropertyResourceFormatter()
        {
            SupportedMediaTypes.Add(MediatTypeName);

        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            //Consider also adding checked for the type of objects.https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/custom-formatters?view=aspnetcore-5.0
            return context.HttpContext.Request.Headers["Content-Type"] == MediatTypeName;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            //Load all the servies needed
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<PropertyResourceFormatter>)) as ILogger;
            var contextAccessor = serviceProvider.GetService(typeof(IActionContextAccessor)) as IActionContextAccessor;
            var urlHelperFactory = serviceProvider.GetService(typeof(IUrlHelperFactory)) as IUrlHelperFactory;
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);

            IDictionary<string, object> resource = new Dictionary<String, object>();

            if (context.ObjectType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                List<Object> resourceList = new List<Object>();

                var list = context.Object as IEnumerable<Property.Api.Features.Property.List.Result.Property>;
                foreach (var item in list)
                {
                    IDictionary<string, object> listItemResource = item.ShapeData(item.GetType());
                    List<Link> links = new List<Link>();
                    links.Add(new Link(urlHelper.Link("Detail", new { PropertyReference = item.PropertyReference }), SelfName, GetMethodName));
                    listItemResource.Add("links", links);
                    resourceList.Add(listItemResource);
                }
                resource.Add("items", resourceList);
            }
            else
            {
                resource = context.Object.ShapeData(context.Object.GetType());
                var property = context.Object as Property.Api.Features.Property.Detail.Result;
                List<Link> links = new List<Link>();
                links.Add(new Link(urlHelper.Link("Detail", new { PropertyReference= property.PropertyReference }), SelfName, GetMethodName));
                resource.Add("links", links);
            }

            var responseText = JsonSerializer.Serialize(resource);
            var response = context.HttpContext.Response;
            response.ContentType = MediatTypeName;
            return response.WriteAsync(responseText);
        }
    }

    public class Link
    {
        public string href { get; private set; }
        public string rel { get; private set; }
        public string method { get; private set; }

        public Link(string rel, string href, string method)
        {
            this.rel = rel;
            this.href = href;
            this.method = method;
        }
    }
}
