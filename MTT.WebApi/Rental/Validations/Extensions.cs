using MTT.Domain.Rental.Entities;
using MTT.WebApi.Rental.DTOs;

namespace MTT.WebApi.Rental.Validations
{
    public static class Extensions
    {
        public static MotoDTO ToDTO(this Moto source)
        {
            return new MotoDTO(source.Id.ToString(), source.Tag, source.Year, source.Model);
        }

        public static MotoboyDTO ToDTO(this Motoboy source)
        {
            return new MotoboyDTO(source.Id.ToString(), source.Name, source.CNPJ, source.DataNascimento, source.CNH);
        }

		public static RentalDTO ToDTO(this Domain.Rental.Entities.Rental source)
		{
			return new RentalDTO(
				source.Id.ToString(),
				source.MotoId.ToString(),
				source.Plan.Id,
				source.MotoboyId.ToString(),
				source.Begin,
				source.End,
				source.Begin.AddDays(source.Plan.Days),
				source.ReturnDate
				);
		}
	}
}
