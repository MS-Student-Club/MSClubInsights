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
    public class CollegeService : GenericService<College>, ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly IMapper _mapper;
        public CollegeService(ICollegeRepository collegeRepository, IMapper mapper) : base(collegeRepository)
        {
            _collegeRepository = collegeRepository;
            _mapper = mapper;
        }
        public async Task<College> AddAsync(CollegeCreateDTO createDTO)
        {
            var existingCollege =
                    await _collegeRepository.GetAsync(u => u.Name == createDTO.Name);

            if (existingCollege != null)
                throw new Exception("A College with the same name already exists.");

            College college = _mapper.Map<College>(createDTO);

            await _collegeRepository.CreateAsync(college);

            return college;
        }
        public async Task<College> UpdateAsync(int id, CollegeUpdateDTO updateDTO)
        {
            var existingCollege = await _collegeRepository.GetAsync(x => x.Id == id);

            if (existingCollege == null)
                throw new ArgumentNullException(nameof(existingCollege));

            if (existingCollege.Name == updateDTO.Name)
                throw new Exception("A College with the same name already exists.");

            _mapper.Map(updateDTO, existingCollege);

            await _collegeRepository.UpdateAsync(existingCollege);

            return existingCollege;
        }
    }
}
