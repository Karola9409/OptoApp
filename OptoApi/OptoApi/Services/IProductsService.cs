using OptoApi.Models;

namespace OptoApi.Services;

public interface IProductsService
{
    List<Product> GetAllProducts();
    OperationResult<Product> GetProduct(int id);
    OperationResult<int> AddProduct(Product product);
    OperationResult UpdateProduct(Product product);
    bool RemoveProduct(int productId);
    bool Exists(string productName);
}