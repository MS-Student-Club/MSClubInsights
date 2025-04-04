using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;


namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly AppDbContext _db;
        public ArticleRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Article article, bool saveImmediately = true)
        {
            dbSet.Update(article);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
