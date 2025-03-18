using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Domain.RepoInterfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository categoryRepository { get; }

        IArticleRepository articleRepository { get; }

        IArticleTagRepository articleTagRepository { get; }

        IRatingRepository ratingRepository { get; }

        ITagRepository tagRepository { get; }

        ILikeRepository likeRepository { get; }

        ICityRepository cityRepository { get; }

        ICommentRepository commentRepository { get; }
        Task SaveChangesAsync();
    }
}
