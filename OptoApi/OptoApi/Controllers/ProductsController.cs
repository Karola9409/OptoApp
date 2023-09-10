using Microsoft.AspNetCore.Mvc;
using OptoApi.ApiModels;
using OptoApi.Models;
using OptoApi.Services;

namespace OptoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(
        IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productsService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var operationResult = await _productsService.GetProduct(id);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok(operationResult.Data);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddProduct([FromBody]ApiRequestProduct product)
    {
        var productToAdd = new Product(
            0,
            product.Name,
            product.Description,
            product.StockCount,
            product.GrossPrice,
            product.VatPercentage,
            product.PhotoUrl);
        
        var operationResult = await _productsService.AddProduct(productToAdd);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok(operationResult.Data);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody] ApiRequestProduct product, int id)
    {
        var productToUpdate = new Product(
                id,
                product.Name,
                product.Description,
                product.StockCount,
                product.GrossPrice,
                product.VatPercentage,
                product.PhotoUrl);
        
        var operationResult = await _productsService.UpdateProduct(productToUpdate);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok();
    }

    [HttpDelete("Remove/{id}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        var operationResult = await _productsService.RemoveProduct(id);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok();
    }
}