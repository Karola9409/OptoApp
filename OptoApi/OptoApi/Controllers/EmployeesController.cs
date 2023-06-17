﻿using System;
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
        public IActionResult AddEmployee([FromBody] ApiRequestEmployee employee)
        {
            try
            {
                var employeeToAdd = new Employee(
                    0,
                    employee.FirstName,
                    employee.LastName,
                    employee.Email,
                    employee.EmployeeRole); 
                
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

