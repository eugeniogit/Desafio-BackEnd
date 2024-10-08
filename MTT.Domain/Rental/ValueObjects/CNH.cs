using MTT.Domain.Shared;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;

namespace MTT.Domain.Rental.ValueObjects
{
    public class CNH : ValueObject
	{
        public int Number { get; set; }
        public CNHCategory Categoty { get; set; }

		public CNH() { }

		public CNH(int number, CNHCategory categoty)
		{
			Number = number;
			Categoty = categoty;
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Number;
		}
	}
}
