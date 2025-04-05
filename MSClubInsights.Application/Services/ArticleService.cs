using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Article;


namespace MSClubInsights.Application.Services
{
    public class ArticleService : GenericService<Article>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        public ArticleService(IArticleRepository articleRepository , IMapper mapper) : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<Article> AddAsync(ArticleCreateDTO createDTO, string userId)
        {
            Article article = _mapper.Map<Article>(createDTO);

            article.AuthorId = userId;

            var ExistingArticle = await _articleRepository.GetAsync(x => x.Title == article.Title);

            if (ExistingArticle != null)
                throw new Exception("An Article with the same name already exists.");

            await _articleRepository.CreateAsync(article);

            return article;
        }

        public async Task<Article> UpdateAsync(int id, string userId, ArticleUpdateDTO updateDTO)
        {
            var ExistingArticle = await _articleRepository.GetAsync(x => x.Id == id);

            if (ExistingArticle == null)
                throw new ArgumentNullException(nameof(ExistingArticle));

            if (ExistingArticle.Title == updateDTO.Title)
                throw new Exception("An Article with the same name already exists.");

            _mapper.Map(updateDTO, ExistingArticle);

            await _articleRepository.UpdateAsync(ExistingArticle);

            return ExistingArticle;
        }
    }
}
