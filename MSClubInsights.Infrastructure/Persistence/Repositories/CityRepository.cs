using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;


namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly AppDbContext _db;
        public CityRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(City city, bool saveImmediately = true)
        {
            dbSet.Update(city);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
