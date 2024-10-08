namespace MTT.WebApi.Rental.Commands
{
	public record AddRental(int RentalPlanId, string MotoboyId, string MotoId, DateTime beginDate);
}
