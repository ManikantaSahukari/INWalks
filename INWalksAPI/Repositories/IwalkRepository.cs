using INWalksAPI.Models.Domain;
using INWalksAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace INWalksAPI.Repositories
{
    public interface IwalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null,string? sortBy= null, bool isAscending= true, int pageNumber=1, int pageSize=1000);
        Task<Walk?> GetWalksByIdAsync(Guid id);
        Task<Walk?> UpdateWalkRequstAsync(Guid id, UpdateWalkRequestDto updateWalkRequestDto);
        Task<Walk?> DeleteWalkByIdAsync(Guid id);

    }
}
