using FluentValidation.Results;
using MTT.Application.Rental.Validations;
using MTT.Domain.Rental.Events;
using MTT.Domain.Rental.ValueObjects;
using MTT.Domain.Shared;
using System.Collections.ObjectModel;

namespace MTT.Domain.Rental.Entities
{
    public class Moto : Entity
    {
        private List<Rental> _rentals = new List<Rental>();
        public string Tag { get; protected set; }
        public int Year { get; protected set; }
        public MotoModel Model { get; protected set; }
        public IReadOnlyCollection<Rental> Rentals => _rentals.ToList();

		public bool IsAvailable => !Rentals.Any(rental => !rental.IsCompleted);

		protected Moto(Guid id, int year, MotoModel model, string tag) : base(id)
		{
			Id = id;
			Year = year;
			Model = model;
			Tag = tag;
		}

		protected Moto() : base(Guid.NewGuid())
		{

		}

        public Moto WithTag(string tag)
        {
            Tag = tag;
			Events.Add(new MotoAddedOrUpdatedDomainEvent(Tag, Year));
			return this;
        }

		public Moto WithRental(Rental rental)
		{
			_rentals.Add(rental);
			return this;
		}

		public static Moto Create(int year, MotoModel model, string tag)
		{
			var moto = new Moto(Guid.NewGuid(), year, model, tag);
			moto.Events.Add(new MotoAddedOrUpdatedDomainEvent(tag, year));
			return moto;
		}

		public override ValidationResult Validation()
		{
			return new MotoValidation().Validate(this);
		}
	}
}
