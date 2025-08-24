using CleanArchitecture.ApplicationCore;
using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Databases
{
    public class ApplicationGenericRepository<TEntity>(ApplicationContext dbContext) : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext _dbContext = dbContext;
        private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

        private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? predicate, string? orderBy = null, string includeProperties = "", bool tracked = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.Sort(orderBy);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        private static async Task<PaginatedList<TEntity>> CreatePaginatedListAsync(IQueryable<TEntity> query, int pageNumber, int pageSize)
        {
            IEnumerable<TEntity> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            int totalCount = await query.CountAsync();
            return new PaginatedList<TEntity>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetQuery(null, orderBy, includeProperties, tracked);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<TEntity>> GetPaginatedAllAsync(int pageNumber = 1, int pageSize = 10, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetQuery(null, orderBy, includeProperties, tracked);
            return await ApplicationGenericRepository<TEntity>.CreatePaginatedListAsync(query, pageNumber, pageSize);
        }

        public async Task<PaginatedList<TEntity>> GetPaginatedAllAsync(QueryStringParameters queryStringParameters, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetQuery(null, queryStringParameters.OrderBy, includeProperties, tracked);
            return await ApplicationGenericRepository<TEntity>.CreatePaginatedListAsync(query, queryStringParameters.PageNumber, queryStringParameters.PageSize);
        }

        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetQuery(predicate, orderBy, includeProperties, tracked);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<TEntity>> GetPaginatedByCondition(Expression<Func<TEntity, bool>> predicate, int pageNumber = 1, int pageSize = 10, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetQuery(predicate, orderBy, includeProperties, tracked);
            return await ApplicationGenericRepository<TEntity>.CreatePaginatedListAsync(query, pageNumber, pageSize);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity,cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities,cancellationToken);
        }

        public void Update(TEntity entity)
        {
            if (_dbSet.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                Update(entity);
            }
        }

        public void Remove(TEntity entity)
        {
            if (_dbSet.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                Remove(entity);
            }
        }

        public void Remove(object id)
        {
            TEntity? entity = _dbSet.Find(id);

            if (entity != null)
            {
                Remove(entity);
            }
        }
    }
}
