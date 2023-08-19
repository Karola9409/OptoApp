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
        private readonly EmployeeValidator _employeeValidator;

        public EmployeesController(
            IEmployeesService employeesService,
            EmployeeValidator employeeValidator)
        {
            _employeesService = employeesService;
            _employeeValidator = employeeValidator;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeesService.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("Get/{id}")]
        public IActionResult GetEmployee(int id)
        {
            var result = _employeesService.GetEmployee(id);
            if (result == null)
            {
                return NotFound($"404, Employee with id {id} not found");
            }
            
            return Ok(result);
        }

        [HttpPost("Add")]
        public IActionResult AddEmployee([FromBody] ApiRequestAddEmployee addEmployee)
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

        [HttpPut("Update/{id}")]
        public IActionResult UpdateEmployee([FromBody] ApiRequestUpdateEmployee employee, int id)
        {
            var existingEmployee = _employeesService.GetEmployee(id);
            if (existingEmployee == null)
            {
                return NotFound($"404, Employee with id {id} not found");
            }
            
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

        [HttpDelete("Remove/{id}")]
        public IActionResult RemoveEmployee(int id)
        {
            var result = _employeesService.GetEmployee(id);
            if (result == null)
            {
                return NotFound($"404, Employee with id {id} not found");
            }
            
            var removed = _employeesService.RemoveEmployee(id);
            return Ok(removed);
        }
    }
}