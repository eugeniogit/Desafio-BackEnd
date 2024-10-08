using FluentValidation;
using MTT.Domain.Rental.Entities;

namespace MTT.Application.Rental.Validations
{
	public class MotoValidation : AbstractValidator<Moto>
	{
		public MotoValidation()
		{
			RuleFor(x => x.Model)
				.NotNull()
				.WithMessage("Model is required");

			RuleFor(x => x.Tag)
				.NotEmpty()
				.WithMessage("Tag is required");

			RuleFor(x => x.Year)
				.GreaterThan(0)
				.WithMessage("Year is required");
		}
	}
}
