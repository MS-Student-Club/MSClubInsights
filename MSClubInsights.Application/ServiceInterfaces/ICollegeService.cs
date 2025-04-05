using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.College;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICollegeService : IGenericService<College>
    {
        Task<College> AddAsync(CollegeCreateDTO createDTO);
        Task<College> UpdateAsync(int id,CollegeUpdateDTO updateDTO);
    }
    
}
