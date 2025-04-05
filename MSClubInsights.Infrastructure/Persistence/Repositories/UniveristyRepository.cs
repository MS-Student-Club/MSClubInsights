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
    public class UniveristyRepository : Repository<Univeristy> , IUniveristyRepository
    {
        private readonly AppDbContext _db;
        public UniveristyRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Univeristy univeristy, bool saveImmediately = true)
        {
            dbSet.Update(univeristy);

            if (saveImmediately)
                await SaveChangesAsync();
        }
    }
}
