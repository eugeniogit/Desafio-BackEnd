using FluentValidation;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.Rental.Commands;

namespace MTT.WebApi.Rental.Validations
{
	public class AddMotoValidation : AbstractValidator<AddMoto>
	{
		public AddMotoValidation()
		{
			RuleFor(x => x.Model)
				.Must(BeValidModel)
					.WithMessage("Model is required");

			RuleFor(x => x.Tag)
				.NotEmpty()
				.WithMessage("Tag is required");

			RuleFor(x => x.Year)
				.GreaterThan(0)
				.WithMessage("Year is required");
		}

		private bool BeValidModel(MotoModel model)
		{
			return Enum.IsDefined(typeof(MotoModel), model);
		}
	}
}
