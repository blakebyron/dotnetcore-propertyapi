using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Property.Infrastructure.Data;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

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
            private readonly MapperConfiguration mapperConfiguration;

            public Handler(PropertyContext context, MapperConfiguration mapperConfiguration)
            {
                this.context = context;
                this.mapperConfiguration = mapperConfiguration;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                Result result = await this.context.Properties.AsNoTracking().ProjectTo<Result>(mapperConfiguration).SingleOrDefaultAsync(f => f.PropertyReference == request.PropertyReference);

                return result;
            }
        }
    }
}
