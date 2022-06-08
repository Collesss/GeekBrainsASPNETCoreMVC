using Repository.Interfaces;
using Repository.Models;

namespace Repository.DataInList
{
    public class ProductRepository : IProductRepository
    {
        private readonly IList<Product> _products;

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

        public async Task<Product> AddAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new ArgumentException("Name can not be null or empty", nameof(entity));

            if (await GetByIdAsync(entity.Id) != null)
                throw new ArgumentException($"Products with Id:{entity.Id}; already exists.");

            if (await GetByNameAsync(entity.Name) != null)
                throw new ArgumentException($"Products with Name:{entity.Name}; already exists.");

            _products.Add(entity);

            return entity;
        }

        public async Task<Product> DeleteAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (await GetByIdAsync(entity.Id) == null)
                throw new ArgumentException($"Products with Id:{entity.Id}; not exists.");

            _products.Remove(entity);

            return entity;
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            Product product = await GetByIdAsync(entity.Id);

            if (product == null)
                throw new ArgumentNullException(nameof(entity));

            if (await GetByIdAsync(entity.Id) == null)
                throw new ArgumentException($"Products with Id:{entity.Id}; not exists.");

            if (await GetByNameAsync(entity.Name) != null)
                throw new ArgumentException($"Products with Name:{entity.Name}; already exists.");

            product.Name = entity.Name;

            return entity;
        }
    }
}
