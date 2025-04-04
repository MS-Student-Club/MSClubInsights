using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ITagService : IGenericService<Tag>
    {
        Task UpdateAsync(Tag tag);
    }
}
