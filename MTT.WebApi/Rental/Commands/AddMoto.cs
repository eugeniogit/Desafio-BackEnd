using MTT.Domain.Rental.ValueObjects;

namespace MTT.WebApi.Rental.Commands
{
	public record AddMoto(string Tag, int Year, MotoModel Model);
}
