using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Rating;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IRatingService : IGenericService<Rating>
    {
        Task<Rating> AddAsync(RatingCreateDTO createDTO, string userId);
        Task<Rating> UpdateAsync(int id , string userId ,RatingUpdateDTO updateDTO);
    }
}
