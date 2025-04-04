using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;


namespace MSClubInsights.Application.Services
{
    public class LikeService : GenericService<Like>, ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        public LikeService(ILikeRepository likeRepository) : base(likeRepository)
        {
            _likeRepository = likeRepository;
        }
    }
}
