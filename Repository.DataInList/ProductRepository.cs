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

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await Task.FromResult(_products);

        public async Task<Product> GetByIdAsync(int id) =>
            await Task.FromResult(_products.FirstOrDefault(product => product.Id == id));

        public async Task<Product> GetByNameAsync(string name) =>
            await Task.FromResult(_products.FirstOrDefault(product => product.Name == name));

        public Task<Product> AddAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_syncObj)
            {
                if (string.IsNullOrWhiteSpace(entity.Name))
                    throw new ArgumentException("Name can not be null or empty", nameof(entity));

                if (GetByIdAsync(entity.Id).Result != null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; already exists.", nameof(entity));

                if (GetByNameAsync(entity.Name).Result != null)
                    throw new ArgumentException($"Products with Name:{entity.Name}; already exists.", nameof(entity));

                _products.Add(entity);
            }

            return Task.FromResult(entity);
        }

        public Task<Product> DeleteAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_syncObj)
            {
                if (GetByIdAsync(entity.Id).Result == null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; not exists.", nameof(entity));

                _products.Remove(entity);
            }

            return Task.FromResult(entity);
        }

        public Task<Product> UpdateAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_syncObj)
            {
                Product product = GetByIdAsync(entity.Id).Result;

                if (product == null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; not exists.", nameof(entity));

                if (GetByNameAsync(entity.Name).Result != null)
                    throw new ArgumentException($"Products with Name:{entity.Name}; already exists.", nameof(entity));

                product.Name = entity.Name;
            }

            return Task.FromResult(entity);
        }
    }
}
