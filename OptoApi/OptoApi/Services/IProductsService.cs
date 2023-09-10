using OptoApi.Models;

namespace OptoApi.Services;

public interface IProductsService
{
    Task<List<Product>> GetAllProducts();
    Task<OperationResult<Product>> GetProduct(int id);
    Task<OperationResult<int>> AddProduct(Product product);
    Task<OperationResult> UpdateProduct(Product product);
    Task<OperationResult<bool>> RemoveProduct(int productId);
    Task<bool> Exists(string productName);
}