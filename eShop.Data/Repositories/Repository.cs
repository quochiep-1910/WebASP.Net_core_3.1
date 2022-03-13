using eShop.Data.EF;
using eShop.Data.Interfaces;
using eShop.Data.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eShop.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly EShopDbContext EShopDbContext;
        public readonly ILogger<Repository<T>> Logger;

        public Repository(EShopDbContext EShopDbContext, ILogger<Repository<T>> logger)
        {
            this.EShopDbContext = EShopDbContext;
            Logger = logger;
        }

        public async Task<T> AddAsync(T entity)
        {
            await EShopDbContext.AddAsync(entity);

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetById(id);
            EShopDbContext.Set<T>().Remove(entity);
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            try
            {
                var result = EShopDbContext.Set<T>().Remove(entity);
                return await Task.FromResult(result.State is EntityState.Deleted);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PaginatedList<T>> GetAllAsync(PagingRequest pagingRequest, Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> query = EShopDbContext.Set<T>();

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (includeProperties != null)
                {
                    query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                }

                return await PaginatedList<T>.ToPaginatedListAsync(query.AsNoTracking(), pagingRequest.PageNumber, pagingRequest.PageSize);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> query = EShopDbContext.Set<T>();

                if (includeProperties != null)
                {
                    query = includeProperties.Aggregate(
                        query,
                        (current, includeProperty) => current.Include(includeProperty)); // chua hieu lam
                }

                return await query.AsNoTracking()
                    .SingleOrDefaultAsync(predicate);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<T> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await EShopDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsyncNoPaging(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> query = EShopDbContext.Set<T>();

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (includeProperties != null)
                {
                    query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                }

                return await query.ToListAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "{0} {1}", "Something went wrong in ", nameof(GetAllAsync));
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            EShopDbContext.Update(entity);
        }
    }
}