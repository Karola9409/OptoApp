using OptoApi.Models;

namespace OptoApi.Repositories;

public interface IProductRepository
{
    Task <List<Product>> GetAllProducts();
    Task <Product?> GetProduct(int id);
    Task<int> AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task<bool> RemoveProduct(int productId);
    Task<bool> Exists(string productName);
}