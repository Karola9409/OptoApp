using Dapper;
using OptoApi.Exceptions;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Validators;

namespace OptoApi.Services;

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductValidator _productValidator;

    public ProductsService(IProductRepository productRepository, ProductValidator productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var result = await _productRepository.GetAllProducts();
        return result.AsList();
    }

    public async Task<OperationResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetProduct(id);
        if (product == null)
        {
            return OperationResult<Product>.Failure($"Product with id {id} not found", ErrorStatus.NotFound);
        }
        return OperationResult<Product>.Success(product);
    }
    
    public async Task<OperationResult<int>> AddProduct(Product product)
    {  
        var productWithGivenNameAlreadyAdded = await _productRepository.Exists(product.Name);
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
        var result = await _productRepository.AddProduct(product);
        return OperationResult<int>.Success(result);
    }
    public async Task<OperationResult> UpdateProduct(Product product)
    {
        var result = await _productRepository.GetProduct(product.Id);
        if (result == null)
        {
            return OperationResult.Failure($"Product with id {product.Id} not found", ErrorStatus.NotFound);
        }
        var validationResult = _productValidator.IsValid(product);
        if (validationResult.IsValid is false)
        {
            return OperationResult.Failure($"Product is invalid:", ErrorStatus.NotValid);
        }
        try
        {
            await _productRepository.UpdateProduct(product);
            return OperationResult.Success();
        }
        catch (ProductNameDuplicateException)
        {
            return OperationResult.Failure($"Product with this name already exist", ErrorStatus.AlreadyExists);
        }
    }
    public async Task<OperationResult<bool>> RemoveProduct(int productId)
    {
        var product = await _productRepository.GetProduct(productId);
        if(product == null)
        {
            return OperationResult<bool>.Failure(
                $"Product with id {productId} not found", 
                ErrorStatus.NotFound);
        }
        var result = _productRepository.RemoveProduct(productId);
        return OperationResult<bool>.Success(await result);

    }
    public async Task<bool> Exists(string productName)
    {
        var result = await _productRepository.Exists(productName);
        return result;
    }
}