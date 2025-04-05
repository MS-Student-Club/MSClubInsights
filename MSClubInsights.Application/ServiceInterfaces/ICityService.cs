using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.City;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICityService : IGenericService<City>
    {
        Task<City> AddAsync(CityCreateDTO createDTO);

        Task<City> UpdateAsync(int id, CityUpdateDTO updateDTO);
    }
}
