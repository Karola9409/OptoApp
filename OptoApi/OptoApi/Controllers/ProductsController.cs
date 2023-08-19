using Microsoft.AspNetCore.Mvc;
using OptoApi.ApiModels;
using OptoApi.Exceptions;
using OptoApi.Models;
using OptoApi.Services;
using OptoApi.Validators;

namespace OptoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;
    private readonly ProductValidator _productValidator;

    public ProductsController(
        IProductsService productsService,
        ProductValidator productValidator)
    {
        _productsService = productsService;
        _productValidator = productValidator;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllProducts()
    {
        var products = _productsService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("Get/{id}")]
    public IActionResult GetProduct(int id)
    {
        var result = _productsService.GetProduct(id);
        if (result == null)
        {
            return NotFound($"404, Product with id {id} not found");
        }
        
        return Ok(result);
    }

    [HttpPost("Add")]
    public IActionResult AddProduct([FromBody] ApiRequestProduct product)
    {
        var productWithGivenNameAlreadyAdded = _productsService.Exists(product.Name);
        if (productWithGivenNameAlreadyAdded)
        {
            return BadRequest($"400, product with name {product.Name} already exists");
        }
        
        var productToAdd = new Product(
            0,
            product.Name,
            product.Description,
            product.StockCount,
            product.GrossPrice,
            product.VatPercentage,
            product.PhotoUrl);

        var validationResult = _productValidator.IsValid(productToAdd);
        if (validationResult.IsValid is false)
        {
            return BadRequest($"Product is invalid: {validationResult.ErrorMessage}");
        }

        var productId = _productsService.AddProduct(productToAdd);
        return Ok(productId);
    }

    [HttpPut("Update/{id}")]
    public IActionResult UpdateProduct([FromBody] ApiRequestProduct product, int id)
    {
        try
        {
            var result = _productsService.GetProduct(id);
            if (result == null)
            {
                return NotFound($"404, Product with id {id} not found");
            }

            ;
            var productToUpdate = new Product(
                id,
                product.Name,
                product.Description,
                product.StockCount,
                product.GrossPrice,
                product.VatPercentage,
                product.PhotoUrl);

            var validationResult = _productValidator.IsValid(productToUpdate);
            if (validationResult.IsValid is false)
            {
                return BadRequest($"Product is invalid: {validationResult.ErrorMessage}");
            }

            _productsService.UpdateProduct(productToUpdate);
            return Ok();
        }
        catch (ProductNameDuplicateException)
        {
            return BadRequest("400, Product with this name already exist");
        }
    }

    [HttpDelete("Remove/{id}")]
    public IActionResult RemoveProduct(int id)
    {
        var result = _productsService.GetProduct(id);
        if (result == null)
        {
            return NotFound($"404, Product with id {id} not found");
        }
        
        var removed = _productsService.RemoveProduct(id);
        return Ok(removed);
    }
}