using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracking = false);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(IEnumerable<T> entities);

    }
}
