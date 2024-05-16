using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;

namespace MotorcycleRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliverysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all delivery records.
        /// </summary>
        /// <returns>A list of all delivery entities.</returns>
        /// <response code="200">Returns all delivery records available.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDeliverys()
        {
            var query = new GetAllDeliverysQuery();
            var deliverys = await _mediator.Send(query);
            return Ok(deliverys);
        }

        /// <summary>
        /// Retrieves a specific delivery by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery to retrieve.</param>
        /// <returns>The requested delivery details.</returns>
        /// <response code="200">Returns the requested delivery.</response>
        /// <response code="404">If no delivery is found with the provided ID.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDelivery(string id)
        {
            var query = new GetDeliveryQuery { Id = id };
            var delivery = await _mediator.Send(query);
            if (delivery == null)
                return NotFound($"No delivery found with ID: {id}");
            return Ok(delivery);
        }

        /// <summary>
        /// Registers a new delivery.
        /// </summary>
        /// <param name="command">The command containing the details of the delivery to be created.</param>
        /// <returns>The result of the create operation including the new delivery ID.</returns>
        /// <response code="200">Returns the ID of the created delivery.</response>
        /// <response code="400">Bad Request - error in the data provided.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterDelivery([FromForm] RegisterDeliveryCommand command)
        {
            // Verifica se a imagem da CNH está no formato válido
            if (!command.IsLicenseImageValid())
            {
                ModelState.AddModelError("LicenseImage", "Invalid image format. Only PNG or BMP formats are allowed.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing delivery with the given ID based on the provided details.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery to update.</param>
        /// <param name="command">The command containing the updated details of the delivery.</param>
        /// <returns>A response indicating the result of the update request.</returns>
        /// <response code="200">Returns success with the ID of the updated delivery.</response>
        /// <response code="400">Bad Request - error in the data provided.</response>
        /// <response code="404">Not Found - the delivery could not be found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDelivery(string id, [FromBody] UpdateDeliveryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.Id = id;
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No delivery found with ID {id}.");
            }
        }

        /// <summary>
        /// Deletes a delivery based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery to be deleted.</param>
        /// <returns>A response indicating the result of the delete request.</returns>
        /// <response code="200">Returns success indicating the delivery was deleted.</response>
        /// <response code="404">Not Found - the delivery could not be found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDelivery(string id)
        {
            try
            {
                var command = new DeleteDeliveryCommand { Id = id };
                var result = await _mediator.Send(command);
                return Ok($"Delivery with ID {id} has been deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No delivery found with ID {id}.");
            }
        }
    }
}
