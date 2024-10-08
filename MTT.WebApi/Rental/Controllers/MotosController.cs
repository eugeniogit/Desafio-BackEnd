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
    [Route("motos")]
    public class MotosController : ControllerBase
    {

        private readonly ILogger<MotosController> _logger;
        private readonly IMotoService _service;

        public MotosController(IMotoService service, ILogger<MotosController> logger)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Gets a list of all motos.
        /// </summary>
        /// <returns>A list of motos.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of all motos", Description = "Returns a list of all motos.")]
        [SwaggerResponse(200, "Returns the list of motos")]
		public IActionResult GetMotos()
		{
			var motos = _service.List();
			return Ok(motos.Select(m => m.ToDTO()));
		}

        /// <summary>
        /// Gets a moto by ID.
        /// </summary>
        /// <param name="id">The ID of the moto.</param>
        /// <returns>The moto details.</returns>
        [HttpGet("{id}", Name = "GetMoto")]
        [SwaggerOperation(Summary = "Gets a moto by ID", Description = "Returns the details of a moto by its ID.")]
        [SwaggerResponse(200, "Returns the moto details")]
        [SwaggerResponse(404, "Moto not found")]
        public async Task<IActionResult> GetMotoAsync(string id)
        {
            if (!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
            }

            var moto = await _service.GetAsync(id);
            return moto == null ? NotFound() : Ok(moto.ToDTO());
        }

        /// <summary>
        /// Adds a new moto.
        /// </summary>
        /// <param name="command">The command containing moto details.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adds a new moto", Description = "Creates a new moto with the provided details.")]
        [SwaggerResponse(201, "Moto created successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        public async Task<IActionResult> AddAsync(AddMoto command)
        {
			var validation = new AddMotoValidation().Validate(command);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors.Select(m => m.ErrorMessage));
			}

			var moto = Moto.Create(command.Year, command.Model, command.Tag);

            var result = await _service.AddAsync(moto);

            return result.Match<IActionResult>(
                onSuccess: () => CreatedAtAction("GetMoto", new { id = moto.Id.ToString() }, command),
                onFailure: errors => BadRequest(errors)
            );

        }

        /// <summary>
        /// Updates the tag of a moto.
        /// </summary>
        /// <param name="id">The ID of the moto.</param>
        /// <param name="command">The command containing the new tag.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}/tag")]
        [SwaggerOperation(Summary = "Updates the tag of a moto", Description = "Updates the tag of a moto with the provided details.")]
        [SwaggerResponse(204, "Tag updated successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        [SwaggerResponse(404, "Moto not found")]
        public async Task<IActionResult> UpdateTagAsync(string id, UpdateMotoTag command)
        {
            if (!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
            }

            var result = await _service.UpdateTagAsync(id, command.newTag);

            return result.Match<IActionResult>(
                onSuccess: () => NoContent(),
                onFailure: errors => BadRequest(errors)
            );

        }

        /// <summary>
        /// Deletes a moto by ID.
        /// </summary>
        /// <param name="id">The ID of the moto.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a moto by ID", Description = "Deletes a moto by its ID.")]
        [SwaggerResponse(204, "Moto deleted successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        [SwaggerResponse(404, "Moto not found")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
            }

            var result = await _service.DeleteAsync(id);

            return result.Match<IActionResult>(
                onSuccess: () => NoContent(),
                onFailure: errors => BadRequest(errors)
            );
        }
    }
}
