using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Property.Infrastructure.Data;

namespace Property.Api.Features.Property
{
    public class List
    {
        public class Query : IRequest<Result>
        {
        }


        public class Result
        {
            public ICollection<Property> Items { get; set; }

            public class Property
            {
                public string PropertyReference { get; set; }
                public string PropertyDescription { get; set; }
                public string Udprn { get; set; }
                public string Uprn { get; set; }
                public string AddressLine1 { get; set; }
                public string AddressLine2 { get; set; }
                public string AddressLine3 { get; set; }
                public string AddressLine4 { get; set; }
                public string Town { get; set; }
                public string Postcode { get; set; }
            }

            public Result(ICollection<Property> items)
            {
                this.Items = items;
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PropertyContext context;

            public Handler(PropertyContext context)
            {
                this.context = context;
            }
            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = this.context.Properties.ToList();
                var result = new Result(items.Select(f => new Result.Property() { PropertyReference = f.Reference.Reference, PropertyDescription = f.Description}).ToList());
                return Task.FromResult<Result>(result);
            }
        }
    }
}
