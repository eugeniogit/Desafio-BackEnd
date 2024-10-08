using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTT.Domain.Rental.ValueObjects;

namespace MTT.Infra.Data.Rental.Configurations
{
    public class RentalConfiguration : IEntityTypeConfiguration<Domain.Rental.Entities.Rental>
    {
        public void Configure(EntityTypeBuilder<Domain.Rental.Entities.Rental> builder)
        {
			builder.HasKey(l => l.Id);

			builder.Property(l => l.Id)
				.ValueGeneratedOnAdd();

			builder.Property(l => l.MotoboyId)
				.IsRequired();

			builder.Property(l => l.Begin)
				.IsRequired();

			builder
				.Property(e => e.Plan)
				.IsRequired()
				.HasConversion(
					v => v.Id,
					v => RentalPlan.Parse(v)
				);

			builder.Ignore(e => e.IsCompleted);
		}
    }
}
