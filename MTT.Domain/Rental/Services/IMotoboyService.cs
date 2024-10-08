using FluentResults;
using MTT.Domain.Rental.Entities;

namespace MTT.Domain.Rental.Services
{
    public interface IMotoboyService
    {
		Task<Motoboy?> GetAsync(string id);
		Task<Result> AddAsync(Motoboy motoboy, string CNHImage64Base);
        Task<Result> UpdateCNHAsync(string motoboyId, string cnhImage64Base);
    }
}
