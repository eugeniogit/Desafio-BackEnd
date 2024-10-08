using FluentValidation.Results;
using MTT.Application.Rental.Validations;
using MTT.Domain.Rental.Events;
using MTT.Domain.Rental.ValueObjects;
using MTT.Domain.Shared;

namespace MTT.Domain.Rental.Entities
{
    public class Rental : Entity
    {
        public Guid MotoId { get; protected set; }
        public RentalPlan Plan { get; protected set; }
        public Guid MotoboyId { get; protected set; }
        public DateTime Begin { get; protected set; }
        public DateTime? End { get; protected set; }
		public DateTime? ReturnDate  { get; protected set; }
		public double? Value { get; protected set; }
        public Moto Moto { get; protected set; }
        public Motoboy Motoboy { get; protected set; }

		protected Rental(Guid id, Guid motoId, RentalPlan plan, Guid motoboyId, DateTime begin) : base(id)
		{
			Id = id;
			MotoId = motoId;
			Plan = plan;
			MotoboyId = motoboyId;
			Begin = begin;
		}
		protected Rental() : base(Guid.NewGuid())
		{
		}

		public static Rental Create(Guid motoId, RentalPlan plan, Guid motoboyId, DateTime begin)
        {
            return new Rental(Guid.NewGuid(), motoId, plan, motoboyId, begin.AddDays(1));
        }

		public Rental Complete(DateTime returnDate, double value)
		{
			ReturnDate = returnDate;
			End = returnDate;
			Value = value;
			return this;
		}

		public bool IsCompleted => End.HasValue && ReturnDate.HasValue;

		public override ValidationResult Validation()
		{
			return new RentalValidation().Validate(this);
		}
	}
}
