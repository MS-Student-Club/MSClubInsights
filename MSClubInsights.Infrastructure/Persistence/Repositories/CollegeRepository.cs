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
    public class CollegeRepository : Repository<College> , ICollegeRepository
    {
        private readonly AppDbContext _db;
        public CollegeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(College college, bool saveImmediately = true)
        {
            dbSet.Update(college);

            if (saveImmediately)
                await SaveChangesAsync();
        }
    }
}
