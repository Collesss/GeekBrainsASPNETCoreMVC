using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Repository.DataInConcurrentDictionary
{
    public class ProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<Product, Product> _products;
        private static readonly object _syncObj = new();

        public ProductRepository(ConcurrentDictionary<Product, Product> products)
        {
            _products = products;
        }

        public Task<IEnumerable<Product>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Product>>(_products.Values);

        public Task<Product> GetByIdAsync(int id) =>
            Task.FromResult(_products.Values.FirstOrDefault(product => product.Id == id));

        public Task<Product> GetByNameAsync(string name) =>
             Task.FromResult(_products.Values.FirstOrDefault(product => product.Name == name));

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

                _products.TryAdd(entity, entity);

                return Task.FromResult(entity);
            }
        }

        public Task<Product> DeleteAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_syncObj)
            {
                Product product = GetByIdAsync(entity.Id).Result;

                if (product == null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; not exists.", nameof(entity));

                _products.TryRemove(product, out _);

                return Task.FromResult(product);
            }
        }

        public Task<Product> UpdateAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_syncObj)
            { 
                var product = GetByIdAsync(entity.Id).Result;

                if (product == null)
                    throw new ArgumentException($"Products with Id:{entity.Id}; not exists.", nameof(entity));

                if (GetByNameAsync(entity.Name).Result != null)
                    throw new ArgumentException($"Products with Name:{entity.Name}; already exists.", nameof(entity));

                product.Name = entity.Name;

                return Task.FromResult(entity);
            }
        }
    }
}
