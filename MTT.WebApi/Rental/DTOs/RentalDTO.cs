using MTT.Domain.Rental.ValueObjects;

namespace MTT.WebApi.Rental.DTOs
{
    public record RentalDTO(
        string id,
        string MotoId, 
        int RentalPlanId, 
        string MotoboyId, 
        DateTime Begin,
		DateTime? End,
		DateTime ExpectedEnd,
		DateTime? ReturnDate
		);
}
