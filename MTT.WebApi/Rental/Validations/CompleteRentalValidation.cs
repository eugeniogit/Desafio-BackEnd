using FluentValidation;
using MTT.WebApi.Rental.Commands;

namespace MTT.WebApi.Rental.Validations
{
	public class CompleteRentalValidation : AbstractValidator<CompleteRental>
	{
		public CompleteRentalValidation()
		{
			RuleFor(x => x.ReturnDate)
				.NotEmpty()
				.WithMessage("ReturnDate is required");
		}

	}
}


