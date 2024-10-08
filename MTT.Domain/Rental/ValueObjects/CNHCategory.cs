namespace MTT.Domain.Rental.ValueObjects
{
    public enum CNHCategory
    {
        A,B,C,AB
    }

	public static class CNHCategoryExtensions
	{
		public static bool AllowOnAddingMotoboy(this CNHCategory category)
		{
			return category == CNHCategory.A || category == CNHCategory.B || category == CNHCategory.AB;
		}

		public static bool AllowRent(this CNHCategory category)
		{
			return category == CNHCategory.A;
		}
	}
}
