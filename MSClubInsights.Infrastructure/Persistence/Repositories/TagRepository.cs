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
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly AppDbContext _db;
        public TagRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Tag tag, bool saveImmediately = true)
        {
            dbSet.Update(tag);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
