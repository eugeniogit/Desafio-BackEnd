using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.ValueObjects;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace MTT.Infra.Data.Rental.Configurations
{
    public class MotoboyConfiguration : IEntityTypeConfiguration<Motoboy>
    {
        public void Configure(EntityTypeBuilder<Motoboy> builder)
        {
			builder.HasKey(l => l.Id);

			builder.Property(l => l.Name)
				.IsRequired();

			builder.Property(l => l.DataNascimento)
				.IsRequired();

			builder.Property(l => l.Id)
				.ValueGeneratedOnAdd();

			builder.OwnsOne(p => p.CNH, cnh =>
			{
				cnh.HasIndex(c => c.Number).IsUnique();
			});

			builder.Property(l => l.CNPJ)
				.IsRequired();
	
			builder.HasIndex(e => e.CNPJ)
                .IsUnique();

            builder.HasMany(e => e.Rentals)
                .WithOne(l => l.Motoboy)
			    .HasForeignKey(l => l.MotoboyId);

			builder.Navigation(e => e.Rentals)
		        .UsePropertyAccessMode(PropertyAccessMode.Field);
		}
    }
}

