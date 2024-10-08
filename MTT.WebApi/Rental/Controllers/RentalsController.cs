using Microsoft.AspNetCore.Mvc;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.Rental.Commands;
using MTT.WebApi.Rental.Validations;
using Swashbuckle.AspNetCore.Annotations;

namespace MTT.WebApi.Rental.Controllers
{
    [ApiController]
    [Route("rentals")]
    public class RentalsController : ControllerBase
    {

        private readonly ILogger<RentalsController> _logger;
        private readonly IRentalService _service;

        public RentalsController(IRentalService service, ILogger<RentalsController> logger)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Gets a rental by ID.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <returns>The rental details.</returns>
        [HttpGet("{id}", Name = "GetRental")]
        [SwaggerOperation(Summary = "Gets a rental by ID", Description = "Returns the details of a rental by its ID.")]
        [SwaggerResponse(200, "Returns the rental details")]
        [SwaggerResponse(404, "Rental not found")]
        public async Task<IActionResult> GetRentalAsync(string id)
        {
			if (!Validations.Validations.IsValidGuid(id))
			{
				return NotFound();
			}

            var rental = await _service.GetAsync(id);

			return rental == null ? NotFound() : Ok(rental.ToDTO());
		}

        /// <summary>
        /// Adds a new rental.
        /// </summary>
        /// <param name="command">The command containing rental details.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adds a new rental", Description = "Creates a new rental with the provided details.")]
        [SwaggerResponse(201, "Rental created successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        public async Task<IActionResult> AddAsync(AddRental command)
        {
			var validation = new AddRentalValidation().Validate(command);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors.Select(m => m.ErrorMessage));
			}

            var rentalPlan = RentalPlan.Parse(command.RentalPlanId);

			var result = await _service.AddAsync(rentalPlan, command.MotoboyId, command.MotoId, command.beginDate);

            return result.Match<IActionResult, Guid>(
                onSuccess: () => CreatedAtAction("GetRental", new { id = result.Value }, command),
                onFailure: errors => BadRequest(errors)
            );

        }

        /// <summary>
        /// Completes a rental.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <param name="command">The command containing completion details.</param>
        /// <returns>The result of the completion operation.</returns>
        [HttpPut("{id}/complete")]
        [SwaggerOperation(Summary = "Completes a rental", Description = "Completes a rental with the provided details.")]
        [SwaggerResponse(204, "Rental completed successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        [SwaggerResponse(404, "Rental not found")]
		public async Task<IActionResult> CompleteAsync(string id, CompleteRental command)
		{
            if (!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
            }

            var validation = new CompleteRentalValidation().Validate(command);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors.Select(m => m.ErrorMessage));
			}

			var result = await _service.CompleteAsync(id, command.ReturnDate);

			return result.Match<IActionResult>(
				onSuccess: () => NoContent(),
				onFailure: errors => BadRequest(errors)
			);

		}
	}
}
