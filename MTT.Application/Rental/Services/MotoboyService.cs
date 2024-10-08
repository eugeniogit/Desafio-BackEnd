using FluentResults;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental.ValueObjects;
using MTT.Domain.Shared;

namespace MTT.Application.Rental.Services
{
    public class MotoboyService : IMotoboyService
    {
        private readonly IRentalUnitOfWork _uow;
        private readonly IStorageService _storageService;

		public MotoboyService(IRentalUnitOfWork uow, IStorageService storageService)
        {
            _uow = uow;
			_storageService = storageService;
		}

		Task<Motoboy?> IMotoboyService.GetAsync(string id)
		{
			return _uow.Motoboy.GetAsync(new Guid(id));
		}

		async Task<Result> IMotoboyService.AddAsync(Motoboy motoboy, string cnhImage64Base)
        {
            var validation = motoboy.Validation();

			if (!validation.IsValid)
            {
				return Result.Fail(validation.Errors.Select(m => m.ErrorMessage));
			}

            var existingMotoboy = _uow.Motoboy
                .List()
                .FirstOrDefault(data => data.CNPJ == motoboy.CNPJ || data.CNH.Number == motoboy.CNH.Number);

            if (existingMotoboy?.CNPJ == motoboy.CNPJ)
            {
                return Result.Fail(Errors.CNPJAddedAlready);
            }

            if (existingMotoboy?.CNH.Number == motoboy.CNH.Number)
            {
                return Result.Fail(Errors.CNHAlreadyExist);
            }

            if (!motoboy.CNH.Categoty.AllowOnAddingMotoboy())
            {
                return Result.Fail(Errors.CNHCategoryNotAllowedForRental);
            }

            var cnhUploadResut = await _storageService.UploadAsync(motoboy.Id.ToString(), cnhImage64Base);

            if (cnhUploadResut.IsFailed)
            {
                return Result.Fail(Errors.CNHUploadUnexpectedError);
            }

            await _uow.Motoboy.AddAsync(motoboy);
            await _uow.SaveChangesAsync();

            return Result.Ok();
        }

        async Task<Result> IMotoboyService.UpdateCNHAsync(string motoboyId, string cnhImage64Base)
        {
			var existingMotoboy = await _uow.Motoboy.GetAsync(new Guid(motoboyId));

			if (existingMotoboy is null)
			{
				return Result.Fail(Errors.MotoboyNotFound);
			}

			return await _storageService.UploadAsync(motoboyId, cnhImage64Base);
		}
    }
}
