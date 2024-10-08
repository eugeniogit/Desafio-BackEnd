using Microsoft.EntityFrameworkCore;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Repositories;

namespace MTT.Infra.Data.Rental.Repositories
{
    public class MotoboyRepository : IMotoboyRepository
    {
        private readonly RentalDbContext _context;

        public MotoboyRepository(RentalDbContext context)
        {
            _context = context;
        }
        public async Task<Motoboy> AddAsync(Motoboy motoboy)
        {
			var entity = await _context.AddAsync(motoboy);
			return entity.Entity;
		}

        public async Task<Motoboy?> GetAsync(Guid id)
        {
			return await List().FirstOrDefaultAsync(motoby => motoby.Id == id);
		}

        public IQueryable<Motoboy> List()
        {
            return _context.Set<Motoboy>().AsNoTracking();
        }

        public void Update(Motoboy motoboy)
        {
			_context.Update(motoboy);
		}
    }
}
