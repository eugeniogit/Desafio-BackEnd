using MediatR;
using Microsoft.EntityFrameworkCore;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Repositories;
using MTT.Domain.Shared;
using MTT.Infra.Data.Rental.Repositories;
using System.Reflection;

namespace MTT.Infra.Data.Rental
{
    public class RentalDbContext : DbContext, IRentalUnitOfWork
    {
        private readonly IMediator _mediator;

		public RentalDbContext(DbContextOptions<RentalDbContext> options, IMediator mediator) : base(options)
		{
			_mediator = mediator;

		}
        public IMotoRepository Moto => new MotoRepository(this);

        public IMotoboyRepository Motoboy => new MotoboyRepository(this);

        public IRentalRepository Rental => new RentalRepository(this);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
				{
					modelBuilder.Entity(entityType.ClrType).Ignore("Events");
				}
			}
		}


		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
            var events = ChangeTracker
				.Entries<Entity>()
				.Select(x => x.Entity)
                .Where(m => m.Events.Any())
                .SelectMany(m => m.Events)
				.ToList();

			var result = await base.SaveChangesAsync(cancellationToken);

			foreach (var @event in events)
            {
                await _mediator.Publish(@event, cancellationToken);
			}

            return result;

		}

	}
}
