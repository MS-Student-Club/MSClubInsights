﻿using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using System.Linq.Expressions;


namespace MSClubInsights.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public GenericService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            return await _repository.GetAllAsync(filter , includeProperties);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracking = false)
        {
            return await _repository.GetAsync(filter , includeProperties , isTracking);
        }

        public async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            await _repository.DeleteRangeAsync(entities);
        }
    }
}
