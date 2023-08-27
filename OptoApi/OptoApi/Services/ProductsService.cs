using Dapper;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Validators;

namespace OptoApi.Services;

public class ProductsService : IProductsService
{
    private readonly ProductRepository _productRepository;
    private readonly ProductValidator _productValidator;

    public ProductsService(ProductRepository productRepository, ProductValidator productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public List<Product> GetAllProducts()
    {
        var result = _productRepository.GetAllProducts();
        return result.AsList();
    }

    public OperationResult<Product> GetProduct(int id)
    {
        var product = _productRepository.GetProduct(id);
        if (product == null)
        {
            return OperationResult<Product>.Failure($"Product with id {id} not found", ErrorStatus.NotFound);
        }
        return OperationResult<Product>.Success(product);
    }
    
    public OperationResult<int> AddProduct(Product product)
    {   var productWithGivenNameAlreadyAdded = _productRepository.Exists(product.Name);
        if (productWithGivenNameAlreadyAdded)
        {
            return OperationResult<int>.Failure(
                $"Product with name {product.Name} already exists",
                ErrorStatus.AlreadyExists);
        }
        
        var validationResult = _productValidator.IsValid(product);
        if (validationResult.IsValid is false)
        {
            return OperationResult<int>.Failure(
                $"Product is invalid: {validationResult.ErrorMessage}",
                ErrorStatus.NotValid);
        }
        var result = _productRepository.AddProduct(product);
        return OperationResult<int>.Success(result);
    }
    public OperationResult UpdateProduct(Product product)
    {
        _productRepository.UpdateProduct(product);
        return OperationResult.Success();
    }

    public bool RemoveProduct(int productId)
    {
        var product = _productRepository.GetProduct(productId);
        if(product == null)
        {
            return false;
        }
        return _productRepository.RemoveProduct(productId);
    }
    public bool Exists(string productName)
    {
        var result = _productRepository.Exists(productName);
        return result;
    }
}