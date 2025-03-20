using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.Services
{
    public class ArticleService : GenericService<Article>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository) : base(articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task UpdateAsync(Article article)
        {
            await _articleRepository.UpdateAsync(article);
        }
    }
}
