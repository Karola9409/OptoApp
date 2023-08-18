using OptoApi.Models;

namespace OptoApi.Services;

public interface IProductsService
{
    List<Product> GetAllProducts();
    Product? GetProduct(int id);
    int AddProduct(Product product);
    void UpdateProduct(Product product);
    bool RemoveProduct(int productId);
    bool Exists(string productName);
}