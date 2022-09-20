using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();

            await nZWalksDbContext.Regions.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> RemoveAsync(Guid id)
        {
            var region = await GetAsync(id);

            if (region is null)
            {
                return null;
            }

            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> UpdateAsync(Guid id, Region updatedRegion)
        {
            var region = await GetAsync(id);

            if (region is null)
            {
                return null;
            }

            region.Code = updatedRegion.Code;
            region.Name = updatedRegion.Name;
            region.Area = updatedRegion.Area;
            region.Lat = updatedRegion.Lat;
            region.Long = updatedRegion.Long;
            region.Population = updatedRegion.Population;

            await nZWalksDbContext.SaveChangesAsync();

            return region;
        }
    }
}
