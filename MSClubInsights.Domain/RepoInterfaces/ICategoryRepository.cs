using MSClubInsights.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Domain.RepoInterfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task UpdateAsync(Category category , bool saveImmediately = true);
    }
}
