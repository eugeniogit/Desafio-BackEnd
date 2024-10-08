using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTT.Domain.Rental.ValueObjects;

namespace MTT.Infra.Data.Rental.Configurations
{
    public class MotoConfiguration : IEntityTypeConfiguration<Domain.Rental.Entities.Moto>
    {
        public void Configure(EntityTypeBuilder<Domain.Rental.Entities.Moto> builder)
        {
			builder.HasKey(l => l.Id);

			builder.Property(e => e.Tag)
                .IsRequired();

			builder.Property(e => e.Year)
	            .IsRequired();

			builder.Property(e => e.Model)
				.IsRequired();

			builder.HasIndex(e => e.Tag)
               .IsUnique();

            builder.HasMany(e => e.Rentals)
                .WithOne(l => l.Moto)
                .HasForeignKey(l => l.MotoId);

			builder.Navigation(e => e.Rentals)
				.UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(e => e.IsAvailable);

        }

    }
}
