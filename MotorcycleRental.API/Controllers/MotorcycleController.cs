using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces.Menssaging;
using MotorcycleRental.Domain.Models;

namespace MotorcycleRental.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IKafkaProducer _producer;
        private readonly IKafkaConsumer _consumer;
        public MotorcyclesController(IMediator mediator, IKafkaProducer producer, IKafkaConsumer consumer)
        {
            _mediator = mediator;
            _producer = producer;
            _consumer = consumer;
        }

        

        /// <summary>
        /// Retrieves all motorcycles.
        /// </summary>
        /// <returns>A list of motorcycles.</returns>
        /// <response code="200">Returns all motorcycles.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Motorcycle>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMotorcycles()
        {
            try
            {
                var query = new GetAllMotorcyclesQuery();
                var motorcycles = await _mediator.Send(query);
                return Ok(motorcycles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to retrieve motorcycles.", ex.Message));
            }
        }
        /// <summary>
        /// Retrieves a specific motorcycle by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the motorcycle to retrieve.</param>
        /// <returns>The requested motorcycle details.</returns>
        /// <response code="200">Returns the requested motorcycle.</response>
        /// <response code="404">If no motorcycle is found with the provided ID.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Motorcycle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMotorcycleById(string id)
        {
            try
            {
                var query = new GetMotocycleByIdQuery { Id = id };
                var motorcycle = await _mediator.Send(query);
                if (motorcycle == null)
                    return NotFound($"No motorcycle found with ID: {id}");

                return Ok(motorcycle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to retrieve the motorcycle.", ex.Message));
            }
        }
        /// <summary>
        /// Retrieves a specific motorcycle by licensePlate.
        /// </summary>
        /// <param name="id">The unique identifier of the motorcycle to retrieve.</param>
        /// <returns>The requested motorcycle details.</returns>
        /// <response code="200">Returns the requested motorcycle.</response>
        /// <response code="404">If no motorcycle is found with the provided ID.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet("ByLicensePlate/{licensePlate}")]
        [ProducesResponseType(typeof(Motorcycle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMotorcycleByLicensePlate(string licensePlate)
        {
            try
            {
                var query = new GetMotorcycleByLicensePlateQuery(licensePlate);
                var motorcycle = await _mediator.Send(query);
                if (motorcycle == null)
                    return NotFound($"No motorcycle found with license plate: {licensePlate}");

                return Ok(motorcycle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to retrieve the motorcycle by license plate.", ex.Message));
            }
        }

        /// <summary>
        /// Creates a new motorcycle.
        /// </summary>
        /// <param name="command">The command containing the details to create a motorcycle.</param>
        /// <returns>The ID of the created motorcycle.</returns>
        /// <response code="200">Returns the ID of the created motorcycle.</response>
        /// <response code="400">If the command is invalid.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An unexpected error occurred.", ex.Message));
            }
        }
        /// <summary>
        /// Updates a motorcycle.
        /// </summary>
        /// <param name="id">The unique identifier of the motorcycle to update.</param>
        /// <param name="command">The command containing the updated motorcycle details.</param>
        /// <returns>The result of the update operation.</returns>
        /// <response code="200">Returns success if the motorcycle was updated.</response>
        /// <response code="400">If the command is invalid.</response>
        /// <response code="404">If no motorcycle is found with the given ID.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMotorcycle(string id, [FromBody] UpdateMotorcycleCommand command)
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
                return NotFound($"No motorcycle found with ID: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to update the motorcycle.", ex.Message));
            }
        }

       

        /// <summary>
        /// Deletes a motorcycle based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the motorcycle to delete.</param>
        /// <returns>Success if the motorcycle was deleted.</returns>
        /// <response code="200">Returns success if the motorcycle was deleted.</response>
        /// <response code="404">If no motorcycle is found with the given ID.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMotorcycle(string id)
        {
            try
            {
                var command = new DeleteMotorcycleCommand { Id = id };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No motorcycle found with ID: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to delete the motorcycle.", ex.Message));
            }

        }
        [HttpPost("Publish")]
        public async Task<IActionResult> PublishMessage([FromBody] string message)
        {
            try
            {
                await _producer.SendMessageAsync(message);
                return Ok("Message published successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to publish message.", ex.Message));
            }
        }

        // GET: /Motorcycles/Consume
        [HttpGet("Consume")]
        public async Task<IActionResult> ConsumeMessage()
        {
            try
            {
                await _consumer.ConsumeAsync();
                return Ok("Message consumed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to consume message.", ex.Message));
            }
        }
    }
    }

