namespace MTT.Application.Rental
{
	public class RentalCalculator
	{
		public static double ExtraDaysPenalty => 50;
		public static double CalculatePrice(Domain.Rental.Entities.Rental rental, DateTime returnDate)
		{
			var plan = rental.Plan;
			var totalDays = (returnDate - rental.Begin).TotalDays;

			var returnedOnTime = totalDays == plan.Days;

			if (returnedOnTime)
			{
				return plan.DayValue * plan.Days;
			}

			var price = plan.DayValue * totalDays;

			var returnedLate = totalDays > plan.Days;

			if (returnedLate)
			{
				var difference = totalDays - plan.Days;
				price += (difference * ExtraDaysPenalty);
			}
			else
			{
				var difference = plan.Days - totalDays;
				price += (plan.PenaltyPercente * difference);
			}

			return price;

		}
	}
}
