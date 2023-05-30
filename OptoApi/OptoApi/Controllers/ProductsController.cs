using System;
using Microsoft.AspNetCore.Mvc;
using OptoApi.ApiModels;

namespace OptoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    public ProductsController()
    {
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllProducts()
    {
        return Ok();
    }

    [HttpGet("Get/{id}")]
    public IActionResult GetProduct(int id)
    {
        return Ok();
    }

    [HttpPost("Add")]
    public IActionResult AddProduct([FromBody] ApiRequestProduct product)
    {
        return Ok();
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

