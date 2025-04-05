using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Like;


namespace MSClubInsights.Application.Services
{
    public class LikeService : GenericService<Like>, ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        public LikeService(ILikeRepository likeRepository , IMapper mapper) : base(likeRepository)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        public async Task<Like> AddAsync(LikeCreateDTO createDTO, string userId)
        {
            Like like = _mapper.Map<Like>(createDTO);

            like.UserId = userId;

            var ExistingLike = await _likeRepository.GetAsync(x => x.ArticleId == like.ArticleId);

            if (ExistingLike != null)
                throw new Exception("An Article with the same name already exists.");

            await _likeRepository.CreateAsync(like);

            return like;
        }
    }
}
