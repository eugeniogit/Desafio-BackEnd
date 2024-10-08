using MTT.Domain.Rental.Entities;

namespace MTT.Domain.Rental.Repositories
{
    public interface IRentalRepository
    {
		Task<Entities.Rental?> GetAsync(Guid id);
		Task<Entities.Rental> AddAsync(Entities.Rental rental);
		void Update(Entities.Rental rental);

	}
}