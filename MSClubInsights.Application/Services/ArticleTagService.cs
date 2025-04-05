using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.ArticleTag;


namespace MSClubInsights.Application.Services
{
    public class ArticleTagService : GenericService<ArticleTag>, IArticleTagService
    {
        private readonly IArticleTagRepository _articleTagRepository;
        private readonly IMapper _mapper;

        public ArticleTagService(IArticleTagRepository articleTagRepository , IMapper mapper) : base(articleTagRepository)
        {
            _articleTagRepository = articleTagRepository;
            _mapper = mapper;
        }

        public async Task<ArticleTag> AddAsync(ArticleTagCreateDTO createDTO)
        {
            var existingArticleTag =
                    await _articleTagRepository.GetAsync(u => u.ArticleId == createDTO.ArticleId && u.TagId == createDTO.TagId);

            if (existingArticleTag != null)
                throw new Exception("This Tag Is Already Placed For This Article");

            ArticleTag articleTag = _mapper.Map<ArticleTag>(createDTO);

            await _articleTagRepository.CreateAsync(articleTag);

            return articleTag;
        }
    }
}
