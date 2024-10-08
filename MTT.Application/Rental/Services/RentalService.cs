using FluentResults;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental.ValueObjects;

namespace MTT.Application.Rental.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalUnitOfWork _uow;

        public RentalService(IRentalUnitOfWork uow)
        {
            _uow = uow;
        }

		Task<Domain.Rental.Entities.Rental?> IRentalService.GetAsync(string id)
		{
			return _uow.Rental.GetAsync(new Guid(id));
		}

		async Task<Result<Guid>> IRentalService.AddAsync(RentalPlan plan, string motoboyId, string motoId, DateTime beginDate)
        {
			var motoboy = await _uow.Motoboy.GetAsync(new Guid(motoboyId));

            if (motoboy is null)
            {
                return Result.Fail(Errors.MotoboyNotFound);
            }

            if (!motoboy.CNH.Categoty.AllowRent())
            {
				return Result.Fail(Errors.CNHCategoryNotAllowedForRental);
			}

            var moto = await _uow.Moto.GetAsync(new Guid(motoId));

            if (moto is null)
            {
                return Result.Fail(Errors.MotoNotFound);
            }

            if (!moto.IsAvailable)
            {
				return Result.Fail(Errors.MotoUnavailable);
			}

            var rental = await _uow.Rental.AddAsync(Domain.Rental.Entities.Rental.Create(
                moto.Id,
				plan,
				new Guid(motoboyId),
				beginDate));

            await _uow.SaveChangesAsync();

            return Result.Ok(rental.Id);

        }

		async Task<Result> IRentalService.CompleteAsync(string id, DateTime returnDate)
		{
			var rental = await _uow.Rental.GetAsync(new Guid(id));

			if (rental is null)
			{
				return Result.Fail(Errors.RentalNotFound);
			}

			var rentalValue = RentalCalculator.CalculatePrice(rental, returnDate);

			rental.Complete(returnDate, rentalValue);

			_uow.Rental.Update(rental);

			await _uow.SaveChangesAsync();

			return Result.Ok();

		}
	}
}
