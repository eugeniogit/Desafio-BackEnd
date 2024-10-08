using FluentValidation;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.Rental.Commands;

namespace MTT.WebApi.Rental.Validations
{
	public class AddRentalValidation : AbstractValidator<AddRental>
	{
		public AddRentalValidation()
		{
			RuleFor(x => x.RentalPlanId)
				.Must(BeValidPlan)
					.WithMessage("Plano is required");

			RuleFor(x => x.MotoboyId)
				.Must(m => Validations.IsValidGuid(m))
				.WithMessage("Invalid Motoboy");

            RuleFor(x => x.MotoId)
                .Must(m => Validations.IsValidGuid(m))
                    .WithMessage("Moto is required");
        }

		private bool BeValidPlan(int rentalPlanId)
		{
			return RentalPlan.Parse(rentalPlanId) != null;
		}
	}
}


