using Microsoft.AspNetCore.Mvc;


namespace MTT.WebApi.Rental.Commands
{
	public record CompleteRental([FromBody] DateTime ReturnDate);
}
