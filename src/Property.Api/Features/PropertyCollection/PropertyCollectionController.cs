using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Property.Api.Infrastructure.Mvc;

namespace Property.Api.Features.PropertyCollection
{
    [ApiController]
    [Route(Constants.FeatureRouteName)]
    public class PropertyCollectionController: ControllerBase
    {
        private readonly IMediator mediator;

        public PropertyCollectionController(IMediator mediator)
        {
            this.mediator = mediator ??
                    throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("({propertyreferences})", Name = "GetPropertyCollection")]
        public async Task<IActionResult> GetPropertyCollectionAsync([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<String> propertyreferences)
        {
            if (propertyreferences == null)
            {
                return BadRequest();
            }

            var query = new ListByPropertyReference.Query() { Properties = propertyreferences };
            var model = await mediator.Send(query);


            if (propertyreferences.Count() != model.Items.Count())
            {
                return NotFound();
            }
            return Ok(model.Items);
        }

        [HttpPost(Name = nameof(CreatePropertyCollection))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePropertyCollection([FromBody]IEnumerable<Create.Command.Property> properties)
        {
            var command = new Create.Command(properties);
            var response = await mediator.Send(command);

            var idsAsString = string.Join(",", properties.Select(a => a.PropertyReference));
            return CreatedAtRoute("GetPropertyCollection",
             new { propertyreferences = idsAsString },
              null);
        }
    }
}
