using Microsoft.AspNetCore.Mvc;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.Rental.Commands;
using MTT.WebApi.Rental.Validations;

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

        [HttpGet("{id}", Name = "GetRental")]
        public async Task<IActionResult> GetRentalAsync(string id)
        {
			if (!Validations.Validations.IsValidGuid(id))
			{
				return NotFound();
			}

            var rental = await _service.GetAsync(id);

			return rental == null ? NotFound() : Ok(rental.ToDTO());
		}

        [HttpPost]
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

		[HttpPut("{id}/complete")]
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
