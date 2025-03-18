using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Domain.RepoInterfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T , bool>>?filter = null , string? includeProperties = null , bool isTracking = false);

        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null , bool isTracking = false);

        Task CreateAsync(T entity , bool saveImmediately = true);

        Task DeleteAsync(T entity , bool saveImmediately = true);

        Task DeleteRangeAsync(IEnumerable<T> entities , bool saveImmediately = true);

        Task SaveChangesAsync();
    }
}
