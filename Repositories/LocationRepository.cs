using JobPortal.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LocationRepository(ApplicationDbContext dbContext) {
            this.dbContext=dbContext;
        }

        public async Task<Location> GetLocation(string LocName)
        {
            return await (from l in dbContext.Location where l.Name==LocName select l).FirstOrDefaultAsync();
        }

        public async Task<Location> InsertLocation(string LocName)
        {
            Location location = new Location() { Name = LocName };
            
             dbContext.Location.Add(location);
             dbContext.SaveChanges();
            return location;    
        }
    }
}
