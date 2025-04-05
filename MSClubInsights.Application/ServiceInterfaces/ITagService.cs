using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Tag;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ITagService : IGenericService<Tag>
    {
        Task<Tag> AddAsync(TagCreateDTO createDTO);

        Task<Tag> UpdateAsync(int id , TagUpdateDTO updateDTO);
    }
}
