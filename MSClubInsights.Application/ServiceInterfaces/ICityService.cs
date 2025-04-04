using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICityService : IGenericService<City>
    {
        Task UpdateAsync(City city);
    }
}
