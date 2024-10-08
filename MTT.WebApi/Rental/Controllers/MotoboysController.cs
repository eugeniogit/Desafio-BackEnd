using Microsoft.AspNetCore.Mvc;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental;
using MTT.WebApi.Rental.Commands;
using MTT.WebApi.Rental.Validations;
using Swashbuckle.AspNetCore.Annotations;

namespace MTT.WebApi.Rental.Controllers
{
    [ApiController]
    [Route("motoboys")]
    public class MotoboysController : ControllerBase
    {

        private readonly ILogger<MotoboysController> _logger;
        private readonly IMotoboyService _service;

        public MotoboysController(IMotoboyService service, ILogger<MotoboysController> logger)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Gets a motoboy by ID.
        /// </summary>
        /// <param name="id">The ID of the motoboy.</param>
        /// <returns>The motoboy details.</returns>
        [HttpGet("{id}", Name = "GetMotoboy")]
        [SwaggerOperation(Summary = "Gets a motoboy by ID", Description = "Returns the details of a motoboy by their ID.")]
        [SwaggerResponse(200, "Returns the motoboy details")]
        [SwaggerResponse(404, "Motoboy not found")]
        public async Task<IActionResult> GetMotoboyAsync(string id)
        {
			if(!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
			}

            var motoboy = await _service.GetAsync(id);
            return motoboy == null ? NotFound() : Ok(motoboy.ToDTO());
        }

        /// <summary>
        /// Adds a new motoboy.
        /// </summary>
        /// <param name="command">The command containing motoboy details.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adds a new motoboy", Description = "Creates a new motoboy with the provided details.")]
        [SwaggerResponse(201, "Motoboy created successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        public async Task<IActionResult> AddAsync(AddMotoboy command)
        {
            var validation = new AddMotoboyValidation().Validate(command);

            if(!validation.IsValid)
			{
				return BadRequest(validation.Errors.Select(m => m.ErrorMessage));
			}

			var CNH = new Domain.Rental.ValueObjects.CNH()
            {
                Categoty = command.CNHCategory,
                Number = command.CNHNumber
            };

            var motoby = Motoboy.Create(command.Name, command.CNPJ, command.DataNascimento, CNH);

            var result = await _service.AddAsync(motoby, command.CNHImage64Base);

            return result.Match<IActionResult>(
                onSuccess: () => CreatedAtAction("GetMotoboy", new { id = motoby.Id.ToString() }, command),
                onFailure: errors => BadRequest(errors)
            );

        }

        /// <summary>
        /// Uploads a new CNH for a motoboy.
        /// </summary>
        /// <param name="command">The command containing CNH details.</param>
        /// <returns>The result of the upload operation.</returns>
        [HttpPut("{id}/cnh")]
        [SwaggerOperation(Summary = "Uploads a new CNH for a motoboy", Description = "Updates the CNH of a motoboy with the provided details.")]
        [SwaggerResponse(204, "CNH updated successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        public async Task<IActionResult> UploadCNHAsync(UploadCNH command)
        {
			var validation = new UploadCNHValidation().Validate(command);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors.Select(m => m.ErrorMessage));
			}

			var result = await _service.UpdateCNHAsync(command.id, command.image64Base);

            return result.Match<IActionResult>(
                onSuccess: () => NoContent(),
                onFailure: errors => BadRequest(errors)
            );

        }

    }
}
