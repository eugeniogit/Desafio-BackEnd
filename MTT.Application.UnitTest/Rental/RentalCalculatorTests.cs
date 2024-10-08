using MTT.Domain.Rental.ValueObjects;
using MTT.Application.Rental;
using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;

namespace MTT.Application.UnitTest.Rental
{
	public class RentalCalculatorTests
	{

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public void ShouldReturnCorrectPrice_WhenReturnedOnTime(int rentalPlanId)
		{
			// Arrange
			var rental = Domain.Rental.Entities.Rental.Create(
			Guid.NewGuid(),
			RentalPlan.Parse(rentalPlanId),
			Guid.NewGuid(),
			DateTime.UtcNow);

			var returnDate = rental.Begin.AddDays(rental.Plan.Days);

			// Act
			var price = RentalCalculator.CalculatePrice(rental, returnDate);

			// Assert
			price.Should().Be(rental.Plan.DayValue * rental.Plan.Days);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public void ShouldReturnCorrectPrice_WhenReturnedLate(int rentalPlanId)
		{
			// Arrange
			var rental = Domain.Rental.Entities.Rental.Create(
			Guid.NewGuid(),
			RentalPlan.Parse(rentalPlanId),
			Guid.NewGuid(),
			DateTime.UtcNow);

			var returnDate = rental.Begin.AddDays(rental.Plan.Days + 1);
			// Act
			var price = RentalCalculator.CalculatePrice(rental, returnDate);

			// Assert
			var totalDays = (returnDate - rental.Begin).TotalDays;
			var difference = totalDays - rental.Plan.Days;

			price.Should().Be((rental.Plan.DayValue * totalDays) + (difference * RentalCalculator.ExtraDaysPenalty));

		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public void CalculatePrice_ShouldReturnCorrectPrice_WhenReturnedEarly(int rentalPlanId)
		{
			// Arrange
			var rental = Domain.Rental.Entities.Rental.Create(
			Guid.NewGuid(),
			RentalPlan.Parse(rentalPlanId),
			Guid.NewGuid(),
			DateTime.UtcNow);

			var returnDate = rental.Begin.AddDays(rental.Plan.Days - 1);

			// Act
			var price = RentalCalculator.CalculatePrice(rental, returnDate);

			// Assert
			var totalDays = (returnDate - rental.Begin).TotalDays;
			var difference = rental.Plan.Days - totalDays;
			price.Should().Be((rental.Plan.DayValue * totalDays) + (rental.Plan.PenaltyPercente * difference));
		}

	}
}
