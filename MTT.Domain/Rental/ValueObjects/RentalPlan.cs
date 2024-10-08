namespace MTT.Domain.Rental.ValueObjects
{
    public record RentalPlan(int Id, int Days, double DayValue, double PenaltyPercente)
    {
        public static RentalPlan A => new RentalPlan(1, 7, 30, 0.2);
        public static RentalPlan B => new RentalPlan(2, 15, 28, 0.4);
        public static RentalPlan C => new RentalPlan(3, 30, 22, 0);
        public static RentalPlan D => new RentalPlan(4, 45, 20, 0);
        public static RentalPlan E => new RentalPlan(5, 50, 18, 0);

        public static RentalPlan? Parse(int id)
        {
            if (A.Id == id)
                return A;
            if (B.Id == id)
                return B;
            if (C.Id == id)
                return C;
            if (D.Id == id)
                return D;
            if (E.Id == id)
                return E;
            return null;
        }
    }
}
