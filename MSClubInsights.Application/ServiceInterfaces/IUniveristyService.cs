using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.College;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IUniveristyService : IGenericService<Univeristy>
    {
        Task<Univeristy> AddAsync(UniversityCreateDTO createDTO);
        Task<Univeristy> UpdateAsync(int id, UniveristyUpdateDTO updateDTO);
    }
}
