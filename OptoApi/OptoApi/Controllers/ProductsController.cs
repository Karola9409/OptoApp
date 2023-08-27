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
        var operationResult = _productsService.GetProduct(id);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok(operationResult.Data);
    }

    [HttpPost("Add")]
    public IActionResult AddProduct([FromBody]ApiRequestProduct product)
    {
        var productToAdd = new Product(
            0,
            product.Name,
            product.Description,
            product.StockCount,
            product.GrossPrice,
            product.VatPercentage,
            product.PhotoUrl);
        
        var operationResult = _productsService.AddProduct(productToAdd);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok(operationResult.Data);
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
        var result = _productsService.RemoveProduct(id);
        if (result is false)
        {
            return NotFound($"404, Product with id {id} not found");
        }
        return Ok();
    }
}