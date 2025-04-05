using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Article;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IArticleService : IGenericService<Article>
    {
        Task<Article> AddAsync(ArticleCreateDTO createDTO, string userId);

        Task<Article> UpdateAsync(int id, string userId, ArticleUpdateDTO updateDTO);
    }
}
