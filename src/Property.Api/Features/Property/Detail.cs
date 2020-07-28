using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

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
            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var propertyList = new List<Result>();

                var p1 = new Result()
                {
                    PropertyReference = "P001",
                    AddressLine1 = "1 Our Street",
                    AddressLine2 = "Somewhere",
                    AddressLine3 = "Over There",
                    Town = "Small Town",
                    Postcode = "AB1"
                };

                propertyList.Add(p1);

                var p2 = new Result()
                {
                    PropertyReference = "P002",
                    AddressLine1 = "2 The Street",
                    AddressLine2 = "Nowhere",
                    Town = "Big Town",
                    Postcode = "AB2"
                };
                propertyList.Add(p2);

                var result = propertyList.SingleOrDefault(f => f.PropertyReference == request.PropertyReference);
                return Task.FromResult<Result>(result);
            }
        }
    }
}
