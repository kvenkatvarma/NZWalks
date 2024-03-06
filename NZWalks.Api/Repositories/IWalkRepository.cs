using NZWalks.Api.Models.Domain;

namespace NZWalks.Api.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filtername = null, string? filtervalue = null, string? sortBy = null, bool IsAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetById(Guid id);
        Task<Walk?> Update(Guid id, Walk walk);
        Task<Walk?> Delete(Guid id);    
    }
}
