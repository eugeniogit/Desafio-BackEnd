using FluentResults;
using MTT.Domain.Rental.ValueObjects;

namespace MTT.Domain.Rental.Services
{
    public interface IRentalService
    {
		Task<Entities.Rental?> GetAsync(string id);
		Task<Result<Guid>> AddAsync(RentalPlan plan, string motoboyId, string motoId, DateTime beginDate);
		Task<Result> CompleteAsync(string id, DateTime returnDate);
    }
}
