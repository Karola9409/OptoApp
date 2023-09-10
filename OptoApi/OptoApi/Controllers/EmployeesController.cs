using Microsoft.AspNetCore.Mvc;
using OptoApi.Services;
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
        public EmployeesController(
            IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeesService.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var operationResult =await _employeesService.GetEmployee(id);
            if (operationResult.Succeeded is false)
            {
                return BadRequest(
                    $"Operation result: {operationResult.Status}, {operationResult.Message}");
            }
            return Ok(operationResult.Data);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddEmployee([FromBody]ApiRequestAddEmployee addEmployee)
        {
            var employeeToAdd = new Employee(
                0,
                addEmployee.FirstName,
                addEmployee.LastName,
                addEmployee.Email,
                addEmployee.EmployeeRole);

            var operationResult =await _employeesService.AddEmployee(employeeToAdd);
            if (operationResult.Succeeded is false)
            {
                return BadRequest(
                    $"Operation result: {operationResult.Status}, {operationResult.Message}");
            }
            return Ok(operationResult.Data);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] ApiRequestUpdateEmployee employee, int id)
        {
            var employeeToUpdate = new UpdateEmployeeModel(
                id,
                employee.LastName,
                employee.Email,
                employee.EmployeeRole);

            var operationResult =await _employeesService.UpdateEmployee(employeeToUpdate);
            if (operationResult.Succeeded is false)
            {
                return BadRequest(
                    $"Operation result: {operationResult.Status}, {operationResult.Message}");
            }
            return Ok();
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> RemoveEmployee(int id)
        {
            var operationResult = await _employeesService.RemoveEmployee(id);
            if (operationResult.Succeeded is false)
            {
                return BadRequest(
                    $"Operation result: {operationResult.Status}, {operationResult.Message}");
            }
            return Ok();
        }
    }
}