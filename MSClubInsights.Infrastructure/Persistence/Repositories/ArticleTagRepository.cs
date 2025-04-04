using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;


namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class ArticleTagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        private readonly AppDbContext _db;
        public ArticleTagRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(ArticleTag articleTag, bool saveImmediately = true)
        {
            dbSet.Update(articleTag);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
