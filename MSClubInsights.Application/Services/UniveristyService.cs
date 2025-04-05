using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.College;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.Services
{
    public class UniveristyService : GenericService<Univeristy> , IUniveristyService
    {
        private readonly IUniveristyRepository _univeristyRepository;
        private readonly IMapper _mapper;
        public UniveristyService(IUniveristyRepository univeristyRepository , IMapper mapper) : base(univeristyRepository)
        {
            _univeristyRepository = univeristyRepository;
            _mapper = mapper;
        }
        public async Task<Univeristy> AddAsync(UniversityCreateDTO createDTO)
        {
            var existingUniveristy =
                    await _univeristyRepository.GetAsync(u => u.Name == createDTO.Name);

            if (existingUniveristy != null)
                throw new Exception("A Univeristy with the same name already exists.");

            Univeristy univeristy = _mapper.Map<Univeristy>(createDTO);

            await _univeristyRepository.CreateAsync(univeristy);

            return univeristy;
        }
        public async Task<Univeristy> UpdateAsync(int id,UniveristyUpdateDTO updateDTO)
        {
            var existingUniveristy = await _univeristyRepository.GetAsync(x => x.Id == id);

            if (existingUniveristy == null)
                throw new ArgumentNullException(nameof(existingUniveristy));

            if (existingUniveristy.Name == updateDTO.Name)
                throw new Exception("A Univeristy with the same name already exists.");

            _mapper.Map(updateDTO, existingUniveristy);

            await _univeristyRepository.UpdateAsync(existingUniveristy);

            return existingUniveristy;
        }
    }
   
}
