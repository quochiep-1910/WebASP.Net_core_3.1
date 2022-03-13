using eShop.Data.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eShop.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="entity">An entity to add</param>

        Task<T> AddAsync(T entity);

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="entity">An entity to UpdateAsync</param>

        Task UpdateAsync(T entity);


        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="entity">An entity to DeleteAsync</param>

        Task DeleteAsync(int id);

        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="entity">An entity to GetAll</param>

        Task<PaginatedList<T>> GetAllAsync(PagingRequest pagingRequest, Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// This method gets a generic object type class.
        /// Allows filtering the returned data and includes any related data.
        /// </summary>
        /// <param name="predicate">A lambda expression tree for filtering data</param>
        /// <param name="includeProperties">A params of lambda expression tree for including related data</param>
        /// <returns>A generic object type</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="entity">An entity to GetById</param>
        Task<bool> RemoveAsync(T entity);
        Task<T> GetById(int? id);
        /// <summary>
        /// This method gets a paged list of a generic type class.
        /// Allows filtering the returned data and includes any related data.
        /// </summary>
        /// <param name="pagingRequest">A paging request</param>
        /// <param name="predicate">A lambda expression tree for filtering data</param>
        /// <param name="includeProperties">A params of lambda expression tree for including related data</param>
        /// <returns>A list of generic class type</returns>
        Task<List<T>> GetAllAsyncNoPaging(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);

    }
}
