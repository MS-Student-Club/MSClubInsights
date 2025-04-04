using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IRatingService : IGenericService<Rating>
    {
        Task UpdateAsync(Rating rating);
    }
}
