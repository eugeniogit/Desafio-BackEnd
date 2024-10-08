using Microsoft.EntityFrameworkCore;
using MTT.Domain.Rental.Repositories;

namespace MTT.Infra.Data.Rental.Repositories
{
    public class RentalRepository : IRentalRepository
    {

        private readonly RentalDbContext _context;
        public RentalRepository(RentalDbContext context)
        {
            _context = context;
        }
		public async Task<Domain.Rental.Entities.Rental?> GetAsync(Guid id)
		{
			return await _context.Set<Domain.Rental.Entities.Rental>()
				.AsNoTracking()
				.FirstOrDefaultAsync(renal => renal.Id == id);
		}
		public async Task<Domain.Rental.Entities.Rental> AddAsync(Domain.Rental.Entities.Rental rental)
        {
			var entity = await _context.AddAsync(rental);
			return entity.Entity;
		}

		public void Update(Domain.Rental.Entities.Rental rental)
		{
			_context.Update(rental);
		}
	}
}

