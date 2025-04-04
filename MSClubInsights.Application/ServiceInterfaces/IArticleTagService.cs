using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IArticleTagService : IGenericService<ArticleTag>
    {
        Task UpdateAsync(ArticleTag articleTag);
    }
}
