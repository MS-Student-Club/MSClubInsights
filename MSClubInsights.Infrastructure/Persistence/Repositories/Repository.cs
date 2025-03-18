using Microsoft.EntityFrameworkCore;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext db) 
        { 
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null , bool isTracking = false)
        {
            IQueryable<T> query = isTracking ? dbSet : dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);
            
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null , bool isTracking = false)
        {
            IQueryable<T> query = isTracking ? dbSet : dbSet.AsNoTracking();

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }


        public async Task CreateAsync(T entity , bool saveImmediately = true)
        {
            await dbSet.AddAsync(entity);

            if (saveImmediately)
                await SaveChangesAsync();
            
        }

        public async Task DeleteAsync(T entity , bool saveImmediately = true)
        {
            dbSet.Remove(entity);
            if (saveImmediately)
                await SaveChangesAsync();
            
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities , bool saveImmediately = true)
        {
            dbSet.RemoveRange(entities);
            if (saveImmediately)
                await SaveChangesAsync();
            
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        

    }
}
