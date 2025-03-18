using MSClubInsights.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Domain.RepoInterfaces
{
    public interface ICityRepository : IRepository<City>
    {
        Task UpdateAsync(City city , bool saveImmediately = true);
    }
}
