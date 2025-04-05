using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Category;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<Category> AddAsync(CategoryCreateDTO createDTO);

        Task<Category> UpdateAsync(int id, CategoryUpdateDTO updateDTO);
    }
}
