using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;

namespace MotorcycleRental.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all available plans.
        /// </summary>
        /// <returns>A list of all plans.</returns>
        /// <response code="200">Returns all the plans available in the system.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var query = new GetAllPlansQuery();
            var plans = await _mediator.Send(query);
            return Ok(plans);
        }

        /// <summary>
        /// Retrieves a specific plan by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the plan to retrieve.</param>
        /// <returns>The requested plan details.</returns>
        /// <response code="200">Returns the requested plan.</response>
        /// <response code="404">If no plan is found with the provided ID.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(string id)
        {
            var query = new GetPlanByIdQuery { Id = id };
            var plan = await _mediator.Send(query);
            if (plan == null)
                return NotFound($"No plan found with ID: {id}");

            return Ok(plan);
        }

        /// <summary>
        /// Creates a new plan based on the provided details.
        /// </summary>
        /// <param name="command">The command containing the details of the plan to be created.</param>
        /// <returns>A response indicating the result of the create request.</returns>
        /// <response code="200">Returns the ID of the created plan.</response>
        /// <response code="400">Bad Request - error in the data provided.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var planId = await _mediator.Send(command);
            return Ok(planId);
        }

        /// <summary>
        /// Updates an existing plan with the given ID based on the provided details.
        /// </summary>
        /// <param name="id">The unique identifier of the plan to be updated.</param>
        /// <param name="command">The command containing the updated details of the plan.</param>
        /// <returns>A response indicating the result of the update request.</returns>
        /// <response code="200">Returns success with the ID of the updated plan.</response>
        /// <response code="400">Bad Request - error in the data provided.</response>
        /// <response code="404">Not Found - the plan could not be found.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(string id, [FromBody] UpdatePlanCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.Id = id;
            await _mediator.Send(command);
            return Ok("Plan updated successfully.");
        }

        /// <summary>
        /// Deletes a plan based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the plan to be deleted.</param>
        /// <returns>A response indicating the result of the delete request.</returns>
        /// <response code="200">Returns success indicating the plan was deleted.</response>
        /// <response code="404">Not Found - the plan could not be found.</response>
        /// <response code="500">Internal Server Error - an internal error occurred.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(string id)
        {
            var command = new DeletePlanCommand { Id = id };
            await _mediator.Send(command);
            return Ok("Plan deleted successfully.");
        }
    }
}
