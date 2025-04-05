using System.Linq.Expressions;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracking = false);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);

    }
}
