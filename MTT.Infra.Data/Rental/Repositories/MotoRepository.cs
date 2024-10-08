using Microsoft.EntityFrameworkCore;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Repositories;

namespace MTT.Infra.Data.Rental.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly RentalDbContext _context;
        public MotoRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<Moto> AddAsync(Moto moto)
        {
			var entity = await _context.AddAsync(moto);
			return entity.Entity;
		}

        public void Delete(Moto moto)
        {
			_context.Remove(moto);

		}

        public void Update(Moto moto)
        {
			_context.Update(moto);
		}

        public async Task<Moto?> GetAsync(Guid id)
        {
			return await _context.Set<Moto>().FindAsync(id);
		}

		public async Task<Moto?> GetByTagAsync(string tag)
		{
			return await _context.Set<Moto>().FirstOrDefaultAsync(m => m.Tag == tag);
		}

		public IQueryable<Moto> List()
        {
			return _context.Set<Moto>().AsNoTracking();
		}

		public async Task<Moto?> GetAvailableMotoAsync()
		{
			return await List()
				.Include(m => m.Rentals)
                .AsNoTracking()
                .FirstOrDefaultAsync(moto => !moto.Rentals.Any(rental => !rental.IsCompleted));
		}
	}
}
