using FluentValidation;
using MTT.WebApi.Rental.Commands;

namespace MTT.WebApi.Rental.Validations
{
	public class UploadCNHValidation : AbstractValidator<UploadCNH>
	{
		public UploadCNHValidation()
		{
			RuleFor(x => x.id)
				.Must(m => Validations.IsValidGuid(m))
				.WithMessage("Is is required");

			RuleFor(x => x.image64Base)
				.NotEmpty()
				.WithMessage("Image64Base is required");
		}

	}
}


