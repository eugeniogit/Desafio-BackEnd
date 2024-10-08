using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTT.Domain.Rental.Repositories;

namespace MTT.Domain.Rental
{
    public interface IRentalUnitOfWork : IUnitOfWork
	{
        IMotoRepository Moto { get; }
        IMotoboyRepository Motoboy { get; }
        IRentalRepository Rental { get; }
    }
}
