using Repository.Models;

namespace Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<Product> GetByNameAsync(string name);
    }
}
