using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;


namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Category category, bool saveImmediately = true)
        {
            dbSet.Update(category);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
