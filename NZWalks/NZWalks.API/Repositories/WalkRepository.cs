using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();

            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }        

        public async Task<Walk> RemoveAsync(Guid id)
        {
            var walk = await GetAsync(id);

            if (walk is null)
            {
                return null;
            }

            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk updatedWalk)
        {
            var walk = await GetAsync(id);

            if (walk is null)
            {
                return null;
            }

            walk.Name = updatedWalk.Name;
            walk.Length = updatedWalk.Length;
            walk.RegionId = updatedWalk.RegionId;
            walk.WalkDifficultyId = updatedWalk.WalkDifficultyId;

            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }
    }
}
