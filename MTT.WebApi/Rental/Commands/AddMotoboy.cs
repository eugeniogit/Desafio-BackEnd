using MTT.Domain.Rental.ValueObjects;

namespace MTT.WebApi.Rental.Commands
{
	public record AddMotoboy(string Name, string CNPJ, DateTime DataNascimento, int CNHNumber, CNHCategory CNHCategory, string CNHImage64Base);
}
