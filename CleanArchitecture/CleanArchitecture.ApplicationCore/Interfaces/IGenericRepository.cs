using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetPaginatedAllAsync(int pageNumber = 1, int pageSize = 10, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetPaginatedAllAsync(QueryStringParameters queryStringParameters, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetPaginatedByCondition(Expression<Func<TEntity, bool>> predicate, int pageNumber = 1, int pageSize = 10, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Remove(object id);
    }
}
