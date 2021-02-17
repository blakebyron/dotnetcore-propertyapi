using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Property.Api.Features.Property
{
    [ApiController]
    [Route(Constants.FeatureRouteName)]
    //Use controller base as we have no need for rendering views
    public class PropertyController : ControllerBase
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        //[ProducesResponseType(typeof(Detail.Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Detail(Detail.Query query)
        {
            var model = await mediator.Send(query);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        /// <summary>
        /// Create a property with a reference and description
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(Create))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateWithReferenceAndDescription.Command command)
        {
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(Detail), new { PropertyReference = command.PropertyReference }, null);
        }

        /// <summary>
        /// Obtain information about the http options
        /// </summary>
        /// <returns></returns>
        //[HttpOptions]
        //public IActionResult GetPropertyOptions()
        //{
        //    Response.Headers.Add("Allow", "GET,OPTIONS,POST");
        //    return Ok();
        //}

        [HttpPatch("{PropertyReference}")]
        public async Task<IActionResult> PartialPropertyUpdate(string PropertyReference, [FromBody] JsonPatchDocument<PropertyPatchModel> patchDocument)
         {
            var command = new Update.Command(PropertyReference, patchDocument);
            var result = await mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
