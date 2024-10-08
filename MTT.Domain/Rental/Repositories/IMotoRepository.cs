using MTT.Domain.Rental.Entities;

namespace MTT.Domain.Rental.Repositories
{
    public interface IMotoRepository
    {
        Task<Moto> AddAsync(Moto moto);
        IQueryable<Moto> List();
        Task<Moto?> GetAsync(Guid id);
        Task<Moto?> GetByTagAsync(string tag);
		void Update(Moto moto);
        void Delete(Moto moto);
    }
}
