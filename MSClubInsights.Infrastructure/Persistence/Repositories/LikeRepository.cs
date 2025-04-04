using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;


namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly AppDbContext _db;
        public LikeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Like like, bool saveImmediately = true)
        {
            dbSet.Update(like);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
