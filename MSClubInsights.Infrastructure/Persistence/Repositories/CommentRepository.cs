using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;


namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _db;
        public CommentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Comment comment, bool saveImmediately = true)
        {
            dbSet.Update(comment);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }
    }
}
