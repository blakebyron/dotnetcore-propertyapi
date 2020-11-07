using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Property.Api.Features.Property
{
    [ApiController]
    [Route(Constants.FeatureRouteName)]
    //Use controller base as we have no need for rendering views
    public class PropertyController:ControllerBase
    {
        private readonly IMediator mediator;

        public PropertyController(IMediator mediator)
        {
            this.mediator = mediator ??
                    throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get a list of properties
        /// </summary>
        /// <returns>A list of properties containing the ID and description</returns>
        [HttpGet(Name = nameof(List))]
        public async Task<IActionResult> List(List.Query query)
        {
            var model = await mediator.Send(query);
            return Ok(model.Items);
        }

        /// <summary>
        /// Get an individual property based on the unique property reference 
        /// </summary>
        /// <param name="query">Property Reference parameter</param>
        /// <returns>A property containing the ID and description</returns>
        [HttpGet("{PropertyReference}", Name = nameof(Detail))]
        public async Task<IActionResult> Detail(Detail.Query query)
        {
            var model = await mediator.Send(query);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWithReferenceAndDescription.Command command)
        {
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(Detail), new { id = response }, null);
        }
    }
}
