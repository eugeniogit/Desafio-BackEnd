using MTT.Domain.Rental.ValueObjects;

namespace MTT.WebApi.Rental.DTOs
{
    public record MotoDTO(string id, string Tag, int Year, MotoModel Model);
}
