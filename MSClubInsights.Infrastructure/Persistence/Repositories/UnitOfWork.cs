using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IArticleRepository articleRepository { get; private set; }

        public IArticleTagRepository articleTagRepository { get; private set; }

        public ICategoryRepository categoryRepository { get; private set; }

        public ICityRepository cityRepository { get; private set; }

        public ICommentRepository commentRepository { get; private set; }

        public ILikeRepository likeRepository { get; private set; }

        public IRatingRepository ratingRepository { get; private set; }

        public ITagRepository tagRepository { get; private set; }



        public UnitOfWork(AppDbContext db)
        {
            _db = db;

            articleRepository = new ArticleRepository(_db);
            articleTagRepository = new ArticleTagRepository(_db);
            categoryRepository = new CategoryRepository(_db);
            cityRepository = new CityRepository(_db);
            commentRepository = new CommentRepository(_db);
            likeRepository = new LikeRepository(_db);
            ratingRepository = new RatingRepository(_db);
            tagRepository = new TagRepository(_db);
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
