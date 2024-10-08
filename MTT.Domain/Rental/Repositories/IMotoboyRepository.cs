using FluentResults;
using MTT.Domain.Rental.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTT.Domain.Rental.Repositories
{
    public interface IMotoboyRepository
    {
        Task<Motoboy> AddAsync(Motoboy motoboy);
        Task<Motoboy?> GetAsync(Guid motoboyId);
        void Update(Motoboy motoboy);
        IQueryable<Motoboy> List();
    }
}
