using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.ArticleTag;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IArticleTagService : IGenericService<ArticleTag>
    {
        Task<ArticleTag> AddAsync(ArticleTagCreateDTO createDTO);

    }
}
