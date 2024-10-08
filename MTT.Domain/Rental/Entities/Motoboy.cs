using FluentValidation.Results;
using MTT.Application.Rental.Validations;
using MTT.Domain.Rental.ValueObjects;
using MTT.Domain.Shared;
using System.Collections.ObjectModel;

namespace MTT.Domain.Rental.Entities
{
    public class Motoboy : Entity
    {
        private List<Rental> _rentals = new();
        public string Name { get; protected set; }
        public string CNPJ { get; protected set; }
        public DateTime DataNascimento { get; protected set; }
        public CNH CNH { get; protected set; }
        public IReadOnlyCollection<Rental> Rentals => _rentals.AsReadOnly();

		protected Motoboy(string id, string name, string cnpj, DateTime dataNascimento, CNH cnh) : base(new Guid(id))
		{
			Id = new Guid(id);
			Name = name;
			CNPJ = cnpj;
			DataNascimento = dataNascimento;
			CNH = cnh;
		}

		protected Motoboy(string id) : base(new Guid(id))
		{
			Id = new Guid(id);
		}

		protected Motoboy() : base(Guid.NewGuid())
		{
			
		}

		public static Motoboy Create(string name, string cnpj, DateTime dataNascimento, CNH cnh)
		{
			return new Motoboy(Guid.NewGuid().ToString(), name, cnpj, dataNascimento, cnh);
		}

		public Motoboy WithCNPJ(string cnpj)
		{
			CNPJ = cnpj;
			return this;
		}

		public Motoboy WithCNH(CNH cnh)
		{
			CNH = cnh;
			return this;
		}

		public Motoboy WithName(string name)
		{
			Name = name;
			return this;
		}

		public override ValidationResult Validation()
		{
			return new MotoboyValidation().Validate(this);
		}
	}
}
