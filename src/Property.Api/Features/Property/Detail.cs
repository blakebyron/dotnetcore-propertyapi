using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Property.Infrastructure.Data;

namespace Property.Api.Features.Property
{
    public class Detail
    {
        public class Query : IRequest<Result>
        {
            public string PropertyReference { get; set; }
        }


        public class Result
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

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PropertyContext context;

            public Handler(PropertyContext context)
            {
                this.context = context;
            }

            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = this.context.Properties.SingleOrDefault(f => f.Reference.Reference == request.PropertyReference);
                Result result = null;
                if (item !=null)
                {
                    result = new Result()
                    {
                        PropertyReference = item.Reference.Reference,
                        PropertyDescription = item.Description
                    };
                }
                return Task.FromResult<Result>(result);
            }
        }
    }
}
