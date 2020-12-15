using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.Data;

namespace Property.Api.Features.PropertyCollection
{
    public class ListByPropertyReference
    {
        public class Query : IRequest<Result>
        {
            public IEnumerable<string> Properties { get; internal set; }
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
            private readonly MapperConfiguration mapperConfiguration;

            public Handler(PropertyContext context, MapperConfiguration mapperConfiguration)
            {
                this.context = context;
                this.mapperConfiguration = mapperConfiguration;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                //define the order by
                Func<IQueryable<Core.Property>, IQueryable<Core.Property>> orderby = f => f.OrderBy(f => f.Reference.Reference);
                //query the the properties ordering the results then map to specific class
                var itemsToQuery = request.Properties.ToList();
                var items = await orderby(this.context.Properties.Where(f=> itemsToQuery.Contains(f.Reference.Reference)).AsNoTracking()).ProjectTo<Result.Property>(mapperConfiguration).ToListAsync();
                var result = new Result(items);
                return result;
            }
        }
    }
}
