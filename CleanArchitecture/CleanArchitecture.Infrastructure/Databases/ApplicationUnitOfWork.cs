using CleanArchitecture.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Databases
{
    public class ApplicationUnitOfWork(ApplicationContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationContext _dbContext = dbContext;

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new ApplicationGenericRepository<TEntity>(_dbContext);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
