using Microsoft.AspNetCore.Mvc;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental;
using MTT.WebApi.Rental.Commands;
using MTT.WebApi.Rental.Validations;

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

		[HttpGet]
		public IActionResult GetMotos()
		{
			var motos = _service.List();
			return Ok(motos.Select(m => m.ToDTO()));
		}

		[HttpGet("{id}", Name = "GetMoto")]
        public async Task<IActionResult> GetMotoAsync(string id)
        {
            if (!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
            }

            var moto = await _service.GetAsync(id);
            return moto == null ? NotFound() : Ok(moto.ToDTO());
        }

        [HttpPost]
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

        [HttpPut("{id}/tag")]
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

        [HttpDelete("{id}")]
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
