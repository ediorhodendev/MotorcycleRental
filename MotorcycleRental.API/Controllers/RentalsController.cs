using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Models;

namespace MotorcycleRental.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all rentals.
        /// </summary>
        /// <returns>A response containing all rentals.</returns>
        /// <response code="200">Returns all the rentals.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Rental>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRentals()
        {
            var query = new GetAllRentalsQuery();
            var rentals = await _mediator.Send(query);
            return Ok(rentals);
        }
        /// <summary>
        /// Retrieves a specific rental by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the rental to retrieve.</param>
        /// <returns>The requested rental details.</returns>
        /// <response code="200">Returns the requested rental.</response>
        /// <response code="404">If no rental is found with the provided ID.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Rental), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRentalById(string id)
        {
            try
            {
                var query = new GetRentalByIdQuery { Id = id };
                var rental = await _mediator.Send(query);
                if (rental == null)
                    return NotFound($"No rental found with ID: {id}");

                return Ok(rental);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to retrieve the rental.", ex.Message));
            }
        }
        /// <summary>
        /// Creates a new rental based on the provided details.
        /// </summary>
        /// <param name="command">The command containing the details of the rental to be created.</param>
        /// <returns>A response indicating the result of the create request.</returns>
        /// <response code="200">Returns success with the ID of the created rental.</response>
        /// <response code="400">Bad Request - error in the data provided.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ErrorResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing rental with the given ID based on the provided details.
        /// </summary>
        /// <param name="id">The unique identifier of the rental to be updated.</param>
        /// <param name="command">The command containing the updated details of the rental.</param>
        /// <returns>A response indicating the result of the update request.</returns>
        /// <response code="200">Returns success with the ID of the updated rental.</response>
        /// <response code="400">Bad Request - error in the data provided.</response>
        /// <response code="404">Not Found - the rental could not be found.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ErrorResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRental(string id, [FromBody] UpdateRentalCommand command)
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
                return NotFound($"No rental found with ID: {id}");
            }
        }

        /// <summary>
        /// Deletes a rental based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the rental to be deleted.</param>
        /// <returns>A response indicating the result of the delete request.</returns>
        /// <response code="200">Returns success indicating the rental was deleted.</response>
        /// <response code="404">Not Found - the rental could not be found.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRental(string id)
        {
            try
            {
                var command = new DeleteRentalCommand { Id = id };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No rental found with ID: {id}");
            }
        }

        /// <summary>
        /// Calculates the total cost of a rental based on the actual return date.
        /// </summary>
        /// <param name="rentalId">The ID of the rental.</param>
        /// <param name="actualReturnDate">The actual date the motorcycle was returned.</param>
        /// <returns>The total cost of the rental including any penalties or additional fees.</returns>
        [HttpGet("CalculateTotalCost/{rentalId}")]
        public async Task<IActionResult> CalculateTotalCost(string rentalId, [FromQuery] DateTime actualReturnDate)
        {
            try
            {
                var command = new CalculateTotalCostCommand
                {
                    RentalId = rentalId,
                    ActualReturnDate = actualReturnDate
                };

                var totalCost = await _mediator.Send(command);
                return Ok(new { RentalId = rentalId, TotalCost = totalCost });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to calculate the total cost.", ex.Message));
            }
        }
    }

}

