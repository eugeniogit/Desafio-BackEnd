using Microsoft.AspNetCore.Mvc;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental;
using MTT.WebApi.Rental.Commands;
using MTT.WebApi.Rental.Validations;
using FluentValidation;

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

        [HttpGet("{id}", Name = "GetMotoboy")]
        public async Task<IActionResult> GetMotoboyAsync(string id)
        {
			if(!Validations.Validations.IsValidGuid(id))
            {
                return NotFound();
			}

            var motoboy = await _service.GetAsync(id);
            return motoboy == null ? NotFound() : Ok(motoboy.ToDTO());
        }

        [HttpPost]
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

        [HttpPut("{id}/cnh")]
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
