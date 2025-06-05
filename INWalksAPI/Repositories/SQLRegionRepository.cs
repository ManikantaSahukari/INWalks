using INWalksAPI.Data;
using INWalksAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace INWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly INWalksDbContext dbContext;
        public SQLRegionRepository(INWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }  

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
            
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null; // Not found — can't update
            }

            dbContext.Remove(existingRegion);
            await dbContext.SaveChangesAsync();

            return existingRegion;        }

        public async Task<List<Region>> GetAllAsync()
        {
           return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null; // Not found — can't update
            }
            // Update properties
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync(); // Save changes 
            return existingRegion;
        }

    }
}
