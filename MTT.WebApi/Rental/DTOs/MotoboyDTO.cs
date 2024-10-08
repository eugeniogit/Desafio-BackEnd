using MTT.Domain.Rental.ValueObjects;

namespace MTT.WebApi.Rental.DTOs
{
    public record MotoboyDTO(string id, string Name, string CNPJ, DateTime DataNascimento, CNH CNH);
}
