using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IArticleService : IGenericService<Article>
    {
        Task UpdateAsync(Article article);
    }
}
