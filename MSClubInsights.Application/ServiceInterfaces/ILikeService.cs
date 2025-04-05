using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Like;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ILikeService : IGenericService<Like>
    {
        Task<Like> AddAsync(LikeCreateDTO createDTO , string userId);

    }
}
