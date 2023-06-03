using System;
using Microsoft.AspNetCore.Mvc;
using OptoApi.ApiModels;
using OptoApi.Models;
using OptoApi.Services;
using OptoApi.Validators;

namespace OptoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;
    private readonly ILogger<ProductsController> _logger;

    private readonly ProductValidator _productValidator;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _productsService = new ProductsService();
        _productValidator = new ProductValidator();
        _logger = logger;
    }
   
    [HttpGet("GetAll")]
    public IActionResult GetAllProducts()
    {
        try
        {
            var products = _productsService.GetAllProducts();
            return Ok(products);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception");
            return StatusCode(500, ex.Message);
        }   
    }

    [HttpGet("Get/{id}")]
    public IActionResult GetProduct(int id)
    {
        try
        {
            var result = _productsService.GetProduct(id);
            if (result == null)
            {
                return NotFound($"404, Product with id {id} not found");
            };
            return Ok(result);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception");
            return StatusCode(500, ex.Message);
        }

    }

    [HttpPost("Add")]
    public IActionResult AddProduct([FromBody] ApiRequestProduct product)
    {
        try
        {
            var productWithGivenNameAlreadyAdded = _productsService.Exists(product.Name);
            if (productWithGivenNameAlreadyAdded)
            {
                return BadRequest($"400, product with name {product.Name} already exists");
            };
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception");
            return StatusCode(500, ex.Message);
        }
       
    }

    [HttpPut("Update/{id}")]
    public IActionResult UpdateProduct([FromBody] ApiRequestProduct product, int id)
    {
        return Ok();
    }

    [HttpDelete("Remove/{id}")]
    public IActionResult RemoveProduct(int id)
    {
        return Ok();
    }
}

