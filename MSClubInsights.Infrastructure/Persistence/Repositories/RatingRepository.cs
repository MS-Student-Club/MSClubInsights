using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _db;
        public RatingRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Rating rating, bool saveImmediately = true)
        {
            dbSet.Update(rating);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
