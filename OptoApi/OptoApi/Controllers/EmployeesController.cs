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
        private readonly IEmployeesService _employeesService;
        private readonly EmployeeValidator _employeeValidator;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            ILogger<EmployeesController> logger,
            IEmployeesService employeesService,
            EmployeeValidator employeeValidator)
        {
            _employeesService = employeesService;
            _employeeValidator = employeeValidator;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var employees = _employeesService.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                var result = _employeesService.GetEmployee(id);
                if (result == null)
                {
                    return NotFound($"404, Employee with id {id} not found");
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddEmployee([FromBody] ApiRequestAddEmployee addEmployee)
        {
            try
            {
                var employeeToAdd = new Employee(
                    0,
                    addEmployee.FirstName,
                    addEmployee.LastName,
                    addEmployee.Email,
                    addEmployee.EmployeeRole); 
                
                var validationResult = _employeeValidator.IsValid(employeeToAdd);
                if (validationResult.IsValid is false)
                {
                    return BadRequest($"Employee is invalid: {validationResult.ErrorMessage}");
                }
                var employeeId = _employeesService.AddEmployee(employeeToAdd);
                return Ok(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateEmployee([FromBody] ApiRequestUpdateEmployee employee, int id)
        {
            try
            {
                var existingEmployee = _employeesService.GetEmployee(id);
                if (existingEmployee == null )
                {
                    return NotFound($"404, Employee with id {id} not found");
                };
                var employeeToUpdate = new Employee(
                    id,
                    existingEmployee.FirstName,
                    employee.LastName,
                    employee.Email,
                    employee.EmployeeRole,
                    existingEmployee.IsDeleted);
                
                var validationResult = _employeeValidator.IsValid(employeeToUpdate);
                if (validationResult.IsValid is false)
                {
                    return BadRequest($"Employee is invalid: {validationResult.ErrorMessage}");
                }
                _employeesService.UpdateEmployee(employeeToUpdate);
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
                var result = _employeesService.GetEmployee(id);
                if (result == null)
                {
                    return NotFound($"404, Employee with id {id} not found");
                };
                var removed = _employeesService.RemoveEmployee(id);
                return Ok(removed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception");
                return StatusCode(500, ex.Message);
            }
        }
    }

}

