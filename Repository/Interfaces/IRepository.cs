namespace Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        public Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
