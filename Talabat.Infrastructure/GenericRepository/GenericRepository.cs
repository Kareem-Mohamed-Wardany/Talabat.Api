using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Talabat.Infrastructure.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbContext.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
            => await ApplySpecification(spec).AsNoTracking().ToListAsync();

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
            => SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
            => await ApplySpecification(spec).CountAsync();

        public void Add(T entity) => _dbContext.Set<T>().Add(entity);


        public void Update(T entity) => _dbContext.Set<T>().Update(entity);


        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
    }
}
