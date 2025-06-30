using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.ApplicationCore.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Services
{
    public class GenericService<TEntity>(IUnitOfWork unitOfWork, ILogger<GenericService<TEntity>> logger) : IGenericService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<GenericService<TEntity>> _logger = logger;

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add an entity - {Json}", JsonSerializer.Serialize(entity));
            await _unitOfWork.Repository<TEntity>().AddAsync(entity,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Gets all entities.");
            return await _unitOfWork.Repository<TEntity>().GetAllAsync(null,string.Empty,false,cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Gets an entity by ID - {Id}",id);
            return await _unitOfWork.Repository<TEntity>().GetByIdAsync(id,cancellationToken);
        }

        public async Task<PaginatedList<TEntity>> GetPaginatedAllAsync(QueryStringParameters queryStringParameters, CancellationToken cancellationToken = default)
        {
            _logger.BeginScope("Gets paginated entities - {QueryStringParameters}", queryStringParameters);
            return await _unitOfWork.Repository<TEntity>().GetPaginatedAllAsync(queryStringParameters, string.Empty, false, cancellationToken);
        }

        public async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Removes an entity - {Json}", JsonSerializer.Serialize(entity));
            _unitOfWork.Repository<TEntity>().Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveAsync(object id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Removes an entity by ID - {id}",id);
            _unitOfWork.Repository<TEntity>().Remove(id);
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updates an entity - {Json}", JsonSerializer.Serialize(entity));
            _unitOfWork.Repository<TEntity>().Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
