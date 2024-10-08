using FluentValidation;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.Rental.Commands;

namespace MTT.WebApi.Rental.Validations
{
	public class AddMotoboyValidation : AbstractValidator<AddMotoboy>
	{
        public AddMotoboyValidation()
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

			RuleFor(x => x.CNHNumber)
				.GreaterThan(0)
				.WithMessage("CNH number is required");

			RuleFor(x => x.CNHCategory)
				.Must(BeAValidCategory)
					.WithMessage("Invalid CNH category");

			RuleFor(x => x.CNHImage64Base)
				.NotNull()
				.WithMessage("CNH image is required");
		}

		private bool BeAValidCategory(CNHCategory category)
		{
			return Enum.IsDefined(typeof(CNHCategory), category);
		}
	}
}
