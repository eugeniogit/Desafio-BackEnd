using Microsoft.AspNetCore.Mvc;
using MTT.Domain.Rental.ValueObjects;

namespace MTT.WebApi.Rental.Commands
{
	public record UploadCNH([FromRoute] string id, [FromBody] string image64Base);
}
