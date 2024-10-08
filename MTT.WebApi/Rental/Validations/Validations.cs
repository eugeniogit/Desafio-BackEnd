namespace MTT.WebApi.Rental.Validations
{
    public class Validations
    {
        public static bool IsValidGuid(string input)
        {
            return Guid.TryParse(input, out _);
        }
    }
}
