using Repository.Interfaces;
using Repository.Models;

namespace Repository.DataInList
{
    public class ProductRepository : IProductRepository
    {
        private readonly IList<Product> _products;
        private static readonly object _syncObj = new();

        public ProductRepository(IList<Product> products)
        {
            _products = products;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            await Task.FromResult(_products);

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = new CancellationToken()) =>
            await Task.FromResult(_products.FirstOrDefault(product => product.Id == id));

        public async Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken = new CancellationToken()) =>
            await Task.FromResult(_products.FirstOrDefault(product => product.Name == name));

        public Task<Product> AddAsync(Product entity, CancellationToken cancellationToken = new CancellationToken())
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            cancellationToken.ThrowIfCancellationRequested();

            lock (_syncObj)
            {
                if (string.IsNullOrWhiteSpace(entity.Name))
                    throw new ArgumentException("Name can not be null or empty", nameof(entity));

                cancellationToken.ThrowIfCancellationRequested();

                if (GetByIdAsync(entity.Id, cancellationToken).Result != null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; already exists.", nameof(entity));

                cancellationToken.ThrowIfCancellationRequested();

                if (GetByNameAsync(entity.Name, cancellationToken).Result != null)
                    throw new ArgumentException($"Products with Name:{entity.Name}; already exists.", nameof(entity));

                cancellationToken.ThrowIfCancellationRequested();

                _products.Add(entity);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(entity);
        }

        public Task<Product> DeleteAsync(Product entity, CancellationToken cancellationToken = new CancellationToken())
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            cancellationToken.ThrowIfCancellationRequested();

            lock (_syncObj)
            {
                if (GetByIdAsync(entity.Id, cancellationToken).Result == null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; not exists.", nameof(entity));

                cancellationToken.ThrowIfCancellationRequested();

                _products.Remove(entity);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(entity);
        }

        public Task<Product> UpdateAsync(Product entity, CancellationToken cancellationToken = new CancellationToken())
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            cancellationToken.ThrowIfCancellationRequested();

            lock (_syncObj)
            {
                Product product = GetByIdAsync(entity.Id, cancellationToken).Result;

                cancellationToken.ThrowIfCancellationRequested();

                if (product == null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; not exists.", nameof(entity));

                cancellationToken.ThrowIfCancellationRequested();

                if (GetByNameAsync(entity.Name, cancellationToken).Result != null)
                    throw new ArgumentException($"Products with Name:{entity.Name}; already exists.", nameof(entity));

                cancellationToken.ThrowIfCancellationRequested();

                product.Name = entity.Name;
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(entity);
        }
    }
}
