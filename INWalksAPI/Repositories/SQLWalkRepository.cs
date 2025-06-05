using INWalksAPI.Data;
using INWalksAPI.Models.Domain;
using INWalksAPI.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace INWalksAPI.Repositories
{
    public class SQLWalkRepository : IwalkRepository
    {
        private readonly INWalksDbContext dbContext;
        public SQLWalkRepository(INWalksDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }


        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, 
                                                        string? sortBy = null, bool isAscending = true,
                                                        int pageNumber = 1, int pageSize=1000)
        {

            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //FILTER
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) //StringComparison.OrdinalIgnoreCase will not bother about case of letter
                { 
                    walks = walks.Where(x=> x.Name.Contains(filterQuery));
                }
            }
            //SORTING
            if(string.IsNullOrWhiteSpace(sortBy)==false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x=>x.Name):walks.OrderByDescending(x=>x.Name);
                }
                else
                {
                    if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                    }
                }
            }
            //PAGINATION
            var skipResult = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();// AFTER PAGINATION
            //return await walks.ToListAsync(); BEFORE PAGINATION
            //return await dbContext.Walks.ToListAsync();
            //return await dbContext.Walks.Include(x=>x.Difficulty).Include(x=>x.Region).ToListAsync(); For type safe
            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk> GetWalksByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateWalkRequstAsync(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var ExitingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(ExitingWalk == null)
            {
                return null;
            }
            ExitingWalk.Name = updateWalkRequestDto.Name;
            ExitingWalk.Description = updateWalkRequestDto.Description;
            ExitingWalk.LengthInKm = updateWalkRequestDto.LengthInKm;
            ExitingWalk.WalkImageUrl = updateWalkRequestDto.WalkImageUrl;
            //WalkDataModel.Difficulty = Guid.Parse(updateWalkRequestDto.DifficultID);
            ExitingWalk.DifficultyId = updateWalkRequestDto.DifficultyId;
            ExitingWalk.RegionId = updateWalkRequestDto.RegionId;
            await dbContext.SaveChangesAsync();

            return ExitingWalk;


        }

        public async Task<Walk> DeleteWalkByIdAsync(Guid id)
        {
            var WalkDataModel = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (WalkDataModel == null)
            {
                return null;
            }
            dbContext.Walks.Remove(WalkDataModel);
            await dbContext.SaveChangesAsync();

            return WalkDataModel;
        }

    }
}
