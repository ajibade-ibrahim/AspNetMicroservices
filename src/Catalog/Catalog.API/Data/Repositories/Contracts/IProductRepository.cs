using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Entities;

namespace Catalog.API.Data.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);
        Task<IEnumerable<Product>> GetProductByNameAsync(string name);
        Task<bool> UpdateProductAsync(Product product);
    }
}