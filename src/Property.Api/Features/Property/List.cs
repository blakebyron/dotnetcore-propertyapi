using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

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
            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var propertyList = new List<Result.Property>();

                var p1 = new Result.Property()
                {
                    PropertyReference = "P001",
                    AddressLine1 = "1 Our Street",
                    AddressLine2 = "Somewhere",
                    AddressLine3 = "Over There",
                    Town = "Small Town",
                    Postcode = "AB1"
                };

                propertyList.Add(p1);

                var p2 = new Result.Property()
                {
                    PropertyReference = "P002",
                    AddressLine1 = "2 The Street",
                    AddressLine2 = "Nowhere",
                    Town = "Big Town",
                    Postcode = "AB2"
                };
                propertyList.Add(p2);

                var result = new Result(propertyList);

                return Task.FromResult<Result>(result);
            }
        }
    }
}
