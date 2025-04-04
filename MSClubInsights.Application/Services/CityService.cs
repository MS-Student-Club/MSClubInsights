using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;


namespace MSClubInsights.Application.Services
{
    public class CityService : GenericService<City>, ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository) : base(cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task UpdateAsync(City city)
        {
            await _cityRepository.UpdateAsync(city);
        }
    }
}
