using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;

namespace NZWalks.Api.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbcontext;

        public SQLRegionRepository(NZWalksDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbcontext.Regions.AddAsync(region);
            await dbcontext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region= await dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return null;
            }

            dbcontext.Regions.Remove(region);
            await dbcontext.SaveChangesAsync();
            return region;
        }

        public async  Task<List<Region>> GetAllAsync()
        {
            return await dbcontext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIDAsync(Guid id)
        {
            return await dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingregion= await dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existingregion == null)
            {
                return null;
            }
            existingregion.Name = region.Name;
            existingregion.Code = region.Code;
            existingregion.RegionImageUrl = region.RegionImageUrl;

            await dbcontext.SaveChangesAsync();
            return existingregion;
        }
    }
}
