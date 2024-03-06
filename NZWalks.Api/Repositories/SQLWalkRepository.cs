using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;

namespace NZWalks.Api.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbcontext;

        public SQLWalkRepository(NZWalksDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbcontext.Walks.AddAsync(walk);
            await dbcontext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> Delete(Guid id)
        {
            var existingwalk = await dbcontext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingwalk == null)
            {
                return null;
            }
            dbcontext.Walks.Remove(existingwalk);
            await dbcontext.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filtername=null, string? filtervalue=null, string? sortBy = null, bool IsAscending = true,int pageNumber=1,int pageSize=1000)
        {
            var walks = dbcontext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering logic
            if (!string.IsNullOrEmpty(filtername) && !string.IsNullOrEmpty(filtervalue))
            {
              if(filtername.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                  walks=walks.Where(x=>x.Name.Contains(filtervalue));
                }
            }

            //Sorting logic
            if(!string.IsNullOrEmpty(sortBy))
            {
              if(sortBy.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }

                if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Paging Logic
            var Numbers = (pageNumber - 1) * pageSize;

           return await walks.Skip(Numbers).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetById(Guid id)
        {
            return await dbcontext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
           
        }

        public async Task<Walk?> Update(Guid id, Walk walk)
        {
            var existingWalk = await dbcontext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk == null)
            {
                return null;
            }
            existingWalk.Description = walk.Description;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            await dbcontext.SaveChangesAsync();
            return existingWalk;

        }
    }
}
