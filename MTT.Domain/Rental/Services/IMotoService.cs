using FluentResults;
using MTT.Domain.Rental.Entities;

namespace MTT.Domain.Rental.Services
{
    public interface IMotoService
    {
        Task<Result> AddAsync(Moto moto);
        IEnumerable<Moto> List();
        Task<Moto?> GetAsync(string id);
        Task<Result> UpdateTagAsync(string oldTag, string newTag);
        Task<Result> DeleteAsync(string tag);
    }
}
