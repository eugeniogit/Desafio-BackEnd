using FluentValidation;
using MTT.Domain.Rental.Entities;

namespace MTT.Application.Rental.Validations
{
	public class MotoboyValidation : AbstractValidator<Motoboy>
	{
        public MotoboyValidation()
        {
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Name is required");

			RuleFor(x => x.CNPJ)
				.NotEmpty()
				.WithMessage("CNPJ is required");

			RuleFor(x => x.DataNascimento)
				.NotEmpty()
				.WithMessage("DataNascimento is required");

			RuleFor(x => x.CNH)
				.NotNull()
				.WithMessage("CNH is required");
		}
	}
}
