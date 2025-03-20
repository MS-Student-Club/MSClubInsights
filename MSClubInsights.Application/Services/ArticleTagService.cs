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
    public class ArticleTagService : GenericService<ArticleTag>, IArticleTagService
    {
        private readonly IArticleTagRepository _articleTagRepository;

        public ArticleTagService(IArticleTagRepository articleTagRepository) : base(articleTagRepository)
        {
            _articleTagRepository = articleTagRepository;
        }
        public async Task UpdateAsync(ArticleTag articleTag)
        {
            await _articleTagRepository.UpdateAsync(articleTag);
        }
    }
}
