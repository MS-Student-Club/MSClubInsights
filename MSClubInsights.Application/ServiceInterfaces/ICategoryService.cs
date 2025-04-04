using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task UpdateAsync(Category category);
    }
}
