using System;
using Microsoft.AspNetCore.Mvc;
using OptoApi.Services;
using OptoApi.Exceptions;
using OptoApi.Models;
using OptoApi.Validators;
using OptoApi.ApiModels;

namespace OptoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase

    {
        private readonly EmployeesService _employeesService;
        private readonly EmployeeValidator _employeeValidator;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _employeesService = new EmployeesService();
            _employeeValidator = new EmployeeValidator();
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult GetEmployees(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("Add")]
        public IActionResult AddEmployee([FromBody] ApiRequestEmployee employee)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateEmployee([FromBody] ApiRequestEmployee employee, int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("Remove/{id}")]
        public IActionResult RemoveEmployee(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }
        }
    }

}

