using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.City;


namespace MSClubInsights.Application.Services
{
    public class CityService : GenericService<City>, ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        public CityService(ICityRepository cityRepository, IMapper mapper) : base(cityRepository)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<City> AddAsync(CityCreateDTO createDTO)
        {
            var existingCity =
                    await _cityRepository.GetAsync(u => u.Name == createDTO.Name);

            if (existingCity != null)
                throw new Exception("A City with the same name already exists.");

            City city = _mapper.Map<City>(createDTO);

            await _cityRepository.CreateAsync(city);

            return city;
        }

        public async Task<City> UpdateAsync(int id, CityUpdateDTO updateDTO)
        {
            var ExistingCity = await _cityRepository.GetAsync(x => x.Id == id);

            if (ExistingCity == null)
                throw new ArgumentNullException(nameof(ExistingCity));

            if (ExistingCity.Name == updateDTO.Name)
                throw new Exception("A City with the same name already exists.");

            _mapper.Map(updateDTO, ExistingCity);

            await _cityRepository.UpdateAsync(ExistingCity);

            return ExistingCity;
        }
    }
}
