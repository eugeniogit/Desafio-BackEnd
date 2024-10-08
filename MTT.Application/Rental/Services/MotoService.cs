using FluentResults;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;

namespace MTT.Application.Rental.Services
{
    public class MotoService : IMotoService
    {
        private readonly IRentalUnitOfWork _uow;

        public MotoService(IRentalUnitOfWork uow)
        {
            _uow = uow;
        }

        async Task<Result> IMotoService.AddAsync(Moto moto)
        {
			var validation = moto.Validation();

			if (!validation.IsValid)
			{
				return Result.Fail(validation.Errors.Select(m => m.ErrorMessage));
			}

			var existingMoto = await _uow.Moto.GetByTagAsync(moto.Tag);

            if (existingMoto is not null)
            {
                return Result.Fail(Errors.MotoAddedAlready);
            }

            await _uow.Moto.AddAsync(moto);
            await _uow.SaveChangesAsync();

            return Result.Ok();

        }

        IEnumerable<Moto> IMotoService.List()
        {
            return _uow.Moto.List();
        }

        Task<Moto?> IMotoService.GetAsync(string id)
        {
            return _uow.Moto.GetAsync(new Guid(id));
        }

        async Task<Result> IMotoService.UpdateTagAsync(string id, string newTag)
        {
            var existingMoto = await _uow.Moto.GetAsync(new Guid(id));

            if (existingMoto == null)
            {
                return Result.Fail(Errors.MotoNotFound);
			}

            existingMoto.WithTag(newTag);

            _uow.Moto.Update(existingMoto);
            await _uow.SaveChangesAsync();

            return Result.Ok();

        }

        async Task<Result> IMotoService.DeleteAsync(string id)
        {
            var existingMOto = await _uow.Moto.GetAsync(new Guid(id));

            if (existingMOto == null)
            {
				return Result.Fail(Errors.MotoNotFound);
			}

            if (existingMOto.Rentals.Any(rental => !rental.IsCompleted))
            {
				return Result.Fail(Errors.MotoRented);
			}

            _uow.Moto.Delete(existingMOto);
            await _uow.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
